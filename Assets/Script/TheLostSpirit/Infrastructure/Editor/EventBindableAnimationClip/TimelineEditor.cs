using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheLostSpirit.Infrastructure.Editor.EventBindableAnimationClip
{
    public class TimelineEditor : ITimelineController
    {
        const string AnimationEventCallbackName = "AnimationEventCallBack";
        readonly SerializedProperty _clipProperty;
        readonly SerializedProperty _ownerProperty;

        readonly VisualElement _root;

        AnimationClip _currentClip;
        float _currentTime;
        OdinInspectorView _inspectorView;
        bool _isUpdatingFromCode;
        float _lastCachedLength = -1f;
        GameObject _previewTarget;
        int _selectedEventIndex = -1;

        TimelineToolbarView _toolbar;

        TimelineTrackView _trackView;

        public TimelineEditor(VisualElement root, SerializedProperty ownerProp, SerializedProperty clipProp)
        {
            _root = root;
            _ownerProperty = ownerProp;
            _clipProperty = clipProp;

            InitializePreviewTarget();
            BuildUI();
            BindLifeCycle();
            Refresh();
        }

        public float GetCurrentFrameRate()
        {
            return _currentClip ? _currentClip.frameRate : 60f;
        }

        public AnimationClip GetCurrentClip()
        {
            return _currentClip;
        }

        // --- Logic: Time & Selection ---

        public void SetTime(float time)
        {
            if (!_currentClip)
            {
                return;
            }

            time = TimelineMath.ClampTime(time, _currentClip.length);
            time = TimelineMath.SnapTime(time, _currentClip.frameRate);

            if (Mathf.Approximately(_currentTime, time))
            {
                return;
            }

            _currentTime = time;

            // UI Update
            _toolbar?.SetTime(_currentTime, _currentClip.frameRate, _currentClip.length);

            _trackView.SetPlayHead(time);

            // Sampling
            SampleAnimation();
        }

        public void SelectEvent(int index, bool forceRefreshOdin = false)
        {
            _selectedEventIndex = index;
            _trackView.HighlightMarker(index);

            if (index == -1)
            {
                _inspectorView.SetData(null, 0, 0, -1, 0, _previewTarget);
                _trackView.HideDurationBar();

                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip);
            if (index >= 0 && index < events.Length)
            {
                var data = EventDataSerializer.Deserialize(events[index].stringParameter);
                _inspectorView.SetData(data, events[index].time, _currentClip.frameRate, index, events.Length,
                                       _previewTarget, forceRefreshOdin);
                data.ResetSelection();
                _trackView.UpdateDurationVisuals(data, index, events[index].time);

                // Set character target for debug draw
                if (_previewTarget != null)
                {
                    _trackView.SetCharacterTarget(_previewTarget);
                }
            }
        }

        public void MoveEvent(int index, float newTime)
        {
            var events = AnimationUtility.GetAnimationEvents(_currentClip);

            if (index < 0 || index >= events.Length)
            {
                return;
            }

            newTime = TimelineMath.ClampTime(newTime, _currentClip.length);
            newTime = TimelineMath.SnapTime(newTime, _currentClip.frameRate);

            // 檢查重疊 (簡易版)
            if (events.Any(e => e != events[index] && Mathf.Approximately(e.time, newTime)))
            {
                ReloadMarkers();

                return;
            }

            events[index].time = newTime;
            events = events.OrderBy(e => e.time).ToArray();
            UpdateClipEvents(events, "Move Animation Event");

            // Re-fetch to confirm new index
            events = AnimationUtility.GetAnimationEvents(_currentClip);
            var newIndex =
                Array.IndexOf(
                    events,
                    events.First(e => Mathf.Approximately(e.time, newTime) &&
                                      e.functionName == events[index].functionName));

            _selectedEventIndex = newIndex;
            ReloadMarkers();
        }

        void BuildUI()
        {
            _toolbar = new TimelineToolbarView();
            _toolbar.onAddEvent += OnAddEvent;
            _toolbar.onFrameChanged += OnToolbarFrameChanged;

            // 2. Track View (Painter2D Based)
            _trackView = new TimelineTrackView(this);

            // 3. Inspector View (Odin)
            _inspectorView = new OdinInspectorView();
            _inspectorView.onSave += SaveEventData;
            _inspectorView.onNavigate += OnInspectorNavigate;
            _inspectorView.onDelete += OnInspectorDelete;
            _inspectorView.onFrameChanged += OnInspectorFrameChanged;

            _root.Add(_toolbar);
            _root.Add(_trackView);
            _root.Add(_inspectorView);
        }

        void BindLifeCycle()
        {
            _root.RegisterCallback<AttachToPanelEvent>(evt =>
            {
                EditorApplication.update += OnEditorUpdate;
                Undo.undoRedoPerformed += OnUndoRedo;
                AnimationUtility.onCurveWasModified += OnClipModified;
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            });

            _root.RegisterCallback<DetachFromPanelEvent>(evt =>
            {
                // Only unsubscribe Unity editor events - DO NOT call Cleanup() here
                // because Unity may detach/reattach panels during normal operation
                EditorApplication.update -= OnEditorUpdate;
                Undo.undoRedoPerformed -= OnUndoRedo;
                AnimationUtility.onCurveWasModified -= OnClipModified;
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

                // Stop animation mode if active
                if (AnimationMode.InAnimationMode())
                {
                    AnimationMode.StopAnimationMode();
                }
            });
        }

        void InitializePreviewTarget()
        {
            var targetObj = _ownerProperty.serializedObject.targetObject;
            _previewTarget = targetObj switch
            {
                Component component => component.gameObject,
                GameObject go => go,
                _ => null
            };
        }

        public void Refresh()
        {
            _currentClip = _clipProperty.objectReferenceValue as AnimationClip;
            _trackView.SetClip(_currentClip);
            _lastCachedLength = _currentClip ? _currentClip.length : -1f;
            _toolbar.style.display = DisplayStyle.Flex;

            if (_currentClip == null)
            {
                _toolbar.style.display = DisplayStyle.None;
                _inspectorView.SetData(null, 0, 0, -1, 0, _previewTarget);

                return;
            }

            ReloadMarkers();
            SetTime(_currentTime); // Restore play-head visual
        }

        void OnUndoRedo()
        {
            ReloadMarkers();
            if (_selectedEventIndex != -1)
            {
                SelectEvent(_selectedEventIndex, true);
            }
        }

        void OnEditorUpdate()
        {
            // Per-frame debug draw with forced SceneView repaint
            if (_trackView?.DrawDebugRange() == true)
            {
                SceneView.RepaintAll(); // Force SceneView to redraw
            }

            if (!_currentClip)
            {
                return;
            }

            // Detect clip length changes
            if (Mathf.Approximately(_currentClip.length, _lastCachedLength))
            {
                return;
            }

            // Auto-adjust cursor if beyond new length
            if (_currentTime >= _lastCachedLength - 0.01f || _currentTime > _currentClip.length)
            {
                _currentTime = _currentClip.length;
            }

            _lastCachedLength = _currentClip.length;
            Refresh();
        }

        void OnClipModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType type)
        {
            if (_isUpdatingFromCode)
            {
                _isUpdatingFromCode = false;

                return;
            }

            if (clip == _currentClip)
            {
                Refresh();
            }
        }


        void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // CRITICAL: Stop AnimationMode IMMEDIATELY when entering PlayMode
            if (state is PlayModeStateChange.ExitingEditMode or PlayModeStateChange.EnteredPlayMode)
            {
                if (AnimationMode.InAnimationMode())
                {
                    // Re-enable Animator before stopping
                    if (_previewTarget != null)
                    {
                        var animator = _previewTarget.GetComponent<Animator>();
                        if (animator != null) animator.enabled = true;
                    }

                    Debug.Log("[TimelineEditor] Stopping AnimationMode for PlayMode");
                    AnimationMode.StopAnimationMode();
                }
            }

            var isPlaying = Application.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode;

            // Disable UI during play mode
            _trackView?.SetEnabled(!isPlaying);
            _toolbar?.SetEnabled(!isPlaying);

            // Force cleanup before entering play mode
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                Cleanup();
            }
        }


        void SampleAnimation()
        {
            // CRITICAL: Never sample in PlayMode
            if (Application.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.Log("[SampleAnimation] Skipped: PlayMode active");

                // Force stop AnimationMode if somehow still active
                if (AnimationMode.InAnimationMode())
                {
                    Debug.LogWarning("[SampleAnimation] Force stopping AnimationMode in PlayMode!");
                    AnimationMode.StopAnimationMode();
                }

                return;
            }

            if (_previewTarget)
            {
                if (!AnimationMode.InAnimationMode())
                {
                    AnimationMode.StartAnimationMode();
                }

                AnimationMode.SampleAnimationClip(_previewTarget, _currentClip, _currentTime);
                SceneView.RepaintAll();

                // Update character target for debug draw
                _trackView.SetCharacterTarget(_previewTarget);
            }
        }

        // --- Logic: Markers & Events ---

        void ReloadMarkers()
        {
            _trackView.ClearMarkers();

            if (!_currentClip)
            {
                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip);
            for (var i = 0; i < events.Length; i++) _trackView.AddMarker(i, events[i].time, i == _selectedEventIndex);
        }

        void OnAddEvent()
        {
            if (!_currentClip)
            {
                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip).ToList();
            if (events.Any(e => Mathf.Approximately(e.time, _currentTime)))
            {
                Debug.LogWarning("Event already exists at this time.");

                return;
            }

            var newEvt = new AnimationEvent
            {
                time = _currentTime,
                functionName = AnimationEventCallbackName,
                stringParameter = ""
            };

            events.Add(newEvt);
            var sorted = events.OrderBy(e => e.time).ToArray();

            UpdateClipEvents(sorted, "Add Animation Event");

            // Select new event
            var index = Array.IndexOf(sorted, newEvt);
            ReloadMarkers();
            SelectEvent(index);
        }

        void SaveEventData(EventData data)
        {
            if (!_currentClip || _selectedEventIndex == -1)
            {
                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip);

            if (_selectedEventIndex >= events.Length)
            {
                return;
            }

            var str = EventDataSerializer.Serialize(data);
            if (events[_selectedEventIndex].stringParameter == str)
            {
                return;
            }

            events[_selectedEventIndex].stringParameter = str;
            UpdateClipEvents(events, "Modify Event Data");

            // Update duration bar when data changes
            _trackView.UpdateDurationVisuals(data, _selectedEventIndex, events[_selectedEventIndex].time);
        }

        void Cleanup()
        {
            if (_inspectorView != null)
            {
                _inspectorView.onNavigate -= OnInspectorNavigate;
                _inspectorView.onDelete -= OnInspectorDelete;
                _inspectorView.onFrameChanged -= OnInspectorFrameChanged;
                _inspectorView.Dispose();
            }

            if (_toolbar != null)
            {
                _toolbar.onAddEvent -= OnAddEvent;
                _toolbar.onFrameChanged -= OnToolbarFrameChanged;
            }

            if (AnimationMode.InAnimationMode())
            {
                // CRITICAL: Re-enable Animator before stopping AnimationMode
                if (_previewTarget != null)
                {
                    var animator = _previewTarget.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.enabled = true;
                        Debug.Log($"[Cleanup] Re-enabled Animator on {_previewTarget.name}");
                    }
                }

                AnimationMode.StopAnimationMode();
            }
        }

        void UpdateClipEvents(AnimationEvent[] events, string undoName)
        {
            _isUpdatingFromCode = true;
            Undo.RecordObject(_currentClip, undoName);
            AnimationUtility.SetAnimationEvents(_currentClip, events);
            EditorUtility.SetDirty(_currentClip);
        }

        void OnToolbarFrameChanged(int frame)
        {
            var frameToTime = TimelineMath.FrameToTime(frame, GetCurrentFrameRate());
            SetTime(frameToTime);
        }

        // Inspector Callbacks

        void OnInspectorNavigate(int delta)
        {
            var newIndex = _selectedEventIndex + delta;
            var events = AnimationUtility.GetAnimationEvents(_currentClip);
            if (newIndex >= 0 && newIndex < events.Length)
            {
                SelectEvent(newIndex);
            }
        }

        void OnInspectorDelete(int dummy)
        {
            if (_selectedEventIndex == -1 || !_currentClip)
            {
                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip).ToList();
            if (_selectedEventIndex < 0 || _selectedEventIndex >= events.Count())
            {
                return;
            }

            // Double check safety like Legacy
            var evtToKill = events[_selectedEventIndex];
            evtToKill.stringParameter = "";
            events[_selectedEventIndex] = evtToKill;
            events.RemoveAt(_selectedEventIndex);
            UpdateClipEvents(events.ToArray(), "Delete Animation Event");

            _selectedEventIndex = -1;
            ReloadMarkers();
            SelectEvent(-1); // Clear Inspector
        }

        void OnInspectorFrameChanged(int newFrame)
        {
            if (_selectedEventIndex == -1 || !_currentClip)
            {
                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip);
            if (_selectedEventIndex >= events.Length)
            {
                return;
            }

            // Validate frame input
            if (newFrame < 0)
            {
                Debug.LogWarning($"Frame must be >= 0, got {newFrame}");
                SelectEvent(_selectedEventIndex); // Force refresh UI
                return;
            }

            var maxFrame = TimelineMath.TimeToFrame(_currentClip.length, _currentClip.frameRate);
            if (newFrame > maxFrame)
            {
                Debug.LogWarning($"Frame {newFrame} exceeds clip length (max: {maxFrame})");
                SelectEvent(_selectedEventIndex); // Force refresh UI
                return;
            }

            var newTime = TimelineMath.FrameToTime(newFrame, _currentClip.frameRate);

            // Check occupation
            if (events
                .Where((t, i) => i != _selectedEventIndex)
                .Any(t => TimelineMath.TimeToFrame(t.time, _currentClip.frameRate) == newFrame))
            {
                Debug.LogWarning($"Frame {newFrame} occupied.");

                SelectEvent(_selectedEventIndex); // Force refresh UI

                return;
            }

            MoveEvent(_selectedEventIndex, newTime);
        }
    }
}