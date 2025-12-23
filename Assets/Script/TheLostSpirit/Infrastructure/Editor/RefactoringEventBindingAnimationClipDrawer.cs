using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

// 假設您的命名空間
namespace TheLostSpirit.Infrastructure.Editor
{
    // ##################################################################
    // 主入口
    // ##################################################################

    [CustomPropertyDrawer(typeof(RefactoringEventBindableAnimationClip))]
    public class RefactoringEventBindableAnimationClipDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var root = new VisualElement();

            // 1. 繪製預設的 Clip 欄位
            var clipProp  = property.FindPropertyRelative(nameof(EventBindableAnimationClip.animationClip));
            var clipField = new PropertyField(clipProp, "Animation Clip");
            root.Add(clipField);

            // 2. 初始化時間軸編輯器
            var editor = new TimelineEditor(root, property, clipProp);

            // 監聽 Clip 變更以刷新編輯器
            root.TrackPropertyValue(clipProp, _ => editor.Refresh());

            return root;
        }
    }

    // ##################################################################
    // [Controller] 邏輯核心：協調 View 與 Model
    // ##################################################################
    public class TimelineEditor
    {
        private readonly VisualElement      _root;
        private readonly SerializedProperty _ownerProperty;
        private readonly SerializedProperty _clipProperty;

        private TimelineTrackView _trackView;
        private OdinInspectorView _inspectorView;
        private Label             _timeLabel;
        private IntegerField      _frameField;

        private AnimationClip _currentClip;
        private GameObject    _previewTarget;
        private float         _currentTime;
        private int           _selectedEventIndex = -1;

        // Constants
        private const string EVENT_FUNCTION_NAME = "AnimationEventCallBack";

        public TimelineEditor(VisualElement root, SerializedProperty ownerProp, SerializedProperty clipProp) {
            _root          = root;
            _ownerProperty = ownerProp;
            _clipProperty  = clipProp;

            BuildUI();
            BindLifeCycle();
            Refresh();
        }

        private void BuildUI() {
            // Container Style
            var container = new VisualElement {
                style = {
                    marginTop         = 5,
                    marginBottom      = 5,
                    borderTopWidth    = 1, borderBottomWidth = 1,
                    borderTopColor    = new Color(0.1f, 0.1f, 0.1f),
                    borderBottomColor = new Color(0.1f, 0.1f, 0.1f),
                    backgroundColor   = new Color(0.2f, 0.2f, 0.2f)
                }
            };

            // 1. Toolbar
            var toolbar = new VisualElement {
                style = {
                    flexDirection = FlexDirection.Row, paddingLeft = 5, paddingRight = 5, height = 25,
                    alignItems    = Align.Center, justifyContent   = Justify.SpaceBetween
                }
            };

            var leftTools = new VisualElement { style     = { flexDirection = FlexDirection.Row } };
            var addBtn    = new Button(OnAddEvent) { text = "+", tooltip = "Add Event at Playhead" };
            addBtn.style.width = 25;
            leftTools.Add(addBtn);

            var rightTools = new VisualElement
                { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } };
            _timeLabel  = new Label("0.00s") { style = { width = 50, unityTextAlign = TextAnchor.MiddleRight } };
            _frameField = new IntegerField { style   = { width = 50, marginLeft     = 5 } };
            _frameField.RegisterValueChangedCallback(evt => SetTime(
                                                         TimelineMath.FrameToTime(evt.newValue, GetFrameRate())));

            rightTools.Add(_timeLabel);
            rightTools.Add(_frameField);
            rightTools.Add(new Label("f"));

            toolbar.Add(leftTools);
            toolbar.Add(rightTools);

            // 2. Track View (Painter2D Based)
            _trackView              = new TimelineTrackView(this);
            _trackView.style.height = 40;

            // 3. Inspector View (Odin)
            _inspectorView = new OdinInspectorView(SaveEventData);

            container.Add(toolbar);
            container.Add(_trackView);
            container.Add(_inspectorView);
            _root.Add(container);
        }

        private void BindLifeCycle() {
            _root.RegisterCallback<AttachToPanelEvent>(evt => {
                EditorApplication.update += OnEditorUpdate;
                Undo.undoRedoPerformed   += OnUndoRedo;
            });
            _root.RegisterCallback<DetachFromPanelEvent>(evt => {
                EditorApplication.update -= OnEditorUpdate;
                Undo.undoRedoPerformed   -= OnUndoRedo;
                Cleanup();
            });
        }

        private float GetFrameRate() => _currentClip ? _currentClip.frameRate : 60f;

        public void Refresh() {
            _currentClip = _clipProperty.objectReferenceValue as AnimationClip;
            _trackView.SetClip(_currentClip);

            if (_currentClip == null) {
                _inspectorView.SetData(null);

                return;
            }

            ReloadMarkers();
            SetTime(_currentTime); // Restore playhead visual
        }

        private void OnUndoRedo() {
            ReloadMarkers();
            if (_selectedEventIndex != -1) SelectEvent(_selectedEventIndex, true);
        }

        private void OnEditorUpdate() {
            // 自動預覽動畫
            if (_currentClip != null && AnimationMode.InAnimationMode()) {
                // 可在此處加入播放邏輯
            }
        }

        // --- Logic: Time & Selection ---

        public void SetTime(float time) {
            if (!_currentClip) return;

            time = TimelineMath.ClampTime(time, _currentClip.length);
            time = TimelineMath.SnapTime(time, _currentClip.frameRate);

            if (Mathf.Approximately(_currentTime, time)) return;

            _currentTime = time;

            // UI Update
            _timeLabel.text = $"{_currentTime:F2}s";
            _frameField.SetValueWithoutNotify(TimelineMath.TimeToFrame(_currentTime, _currentClip.frameRate));
            _trackView.SetPlayhead(time);

            // Sampling
            SampleAnimation();
        }

        private void SampleAnimation() {
            if (_previewTarget == null) {
                var targetObj                 = _ownerProperty.serializedObject.targetObject as Component;
                if (targetObj) _previewTarget = targetObj.gameObject;
            }

            if (_previewTarget && _currentClip) {
                if (!AnimationMode.InAnimationMode()) AnimationMode.StartAnimationMode();
                AnimationMode.SampleAnimationClip(_previewTarget, _currentClip, _currentTime);
            }
        }

        // --- Logic: Markers & Events ---

        private void ReloadMarkers() {
            _trackView.ClearMarkers();

            if (!_currentClip) return;

            var events = AnimationUtility.GetAnimationEvents(_currentClip);
            for (int i = 0; i < events.Length; i++) {
                _trackView.AddMarker(i, events[i].time, i == _selectedEventIndex);
            }
        }

        public void SelectEvent(int index, bool forceRefreshOdin = false) {
            _selectedEventIndex = index;
            _trackView.HighlightMarker(index);

            if (index == -1) {
                _inspectorView.SetData(null);

                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip);
            if (index >= 0 && index < events.Length) {
                SetTime(events[index].time); // Jump to event

                // 只有當真的需要刷新 Odin Inspector 時才反序列化，避免效能浪費
                // forceRefreshOdin 用於 Undo/Redo 後強制重繪
                var data = EventDataSerializer.Deserialize(events[index].stringParameter);
                _inspectorView.SetData(data, forceRefreshOdin);
            }
        }

        public void MoveEvent(int index, float newTime) {
            var events = AnimationUtility.GetAnimationEvents(_currentClip);

            if (index < 0 || index >= events.Length) return;

            newTime = TimelineMath.ClampTime(newTime, _currentClip.length);
            newTime = TimelineMath.SnapTime(newTime, _currentClip.frameRate);

            // 檢查重疊 (簡易版)
            if (events.Any(e => e != events[index] && Mathf.Approximately(e.time, newTime)))
                return;

            Undo.RecordObject(_currentClip, "Move Animation Event");
            events[index].time = newTime;

            // 重新排序並更新索引
            var targetEvent = events[index];
            AnimationUtility.SetAnimationEvents(_currentClip, events);

            // 重新獲取以確認新索引
            var newEvents = AnimationUtility.GetAnimationEvents(_currentClip);
            int newIndex =
                Array.IndexOf(
                    newEvents,
                    newEvents.First(e => Mathf.Approximately(e.time, newTime) &&
                                         e.functionName == targetEvent.functionName));

            _selectedEventIndex = newIndex;
            SetTime(newTime);
            ReloadMarkers();
        }

        private void OnAddEvent() {
            if (!_currentClip) return;

            var events = AnimationUtility.GetAnimationEvents(_currentClip).ToList();
            if (events.Any(e => Mathf.Approximately(e.time, _currentTime))) {
                Debug.LogWarning("Event already exists at this time.");

                return;
            }

            var newEvt = new AnimationEvent {
                time            = _currentTime,
                functionName    = EVENT_FUNCTION_NAME,
                stringParameter = ""
            };

            events.Add(newEvt);
            var sorted = events.OrderBy(e => e.time).ToArray();

            Undo.RecordObject(_currentClip, "Add Animation Event");
            AnimationUtility.SetAnimationEvents(_currentClip, sorted);

            // 選取新事件
            int index = Array.IndexOf(sorted, newEvt);
            ReloadMarkers();
            SelectEvent(index);
        }

        private void SaveEventData(EventData data) {
            if (!_currentClip || _selectedEventIndex == -1) return;

            var events = AnimationUtility.GetAnimationEvents(_currentClip);

            if (_selectedEventIndex >= events.Length) return;

            var str = EventDataSerializer.Serialize(data);
            if (events[_selectedEventIndex].stringParameter != str) {
                Undo.RecordObject(_currentClip, "Modify Event Data");
                events[_selectedEventIndex].stringParameter = str;
                AnimationUtility.SetAnimationEvents(_currentClip, events);
                EditorUtility.SetDirty(_currentClip);
            }
        }

        private void Cleanup() {
            _inspectorView?.Dispose();
            if (AnimationMode.InAnimationMode()) AnimationMode.StopAnimationMode();
        }
    }

    // ##################################################################
    // [View] 標尺軌道與繪圖 (使用 Painter2D)
    // ##################################################################
    public class TimelineTrackView : VisualElement
    {
        private readonly TimelineEditor _controller;
        private          AnimationClip  _clip;
        private          VisualElement  _playhead;
        private          VisualElement  _markerContainer;

        public TimelineTrackView(TimelineEditor controller) {
            _controller           = controller;
            style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            style.overflow        = Overflow.Hidden;
            style.marginTop       = 2;

            // 1. 標尺繪製回調
            generateVisualContent += OnGenerateVisualContent;

            // 2. Marker 容器
            _markerContainer = new VisualElement
                { style = { width = Length.Percent(100), height = Length.Percent(100), position = Position.Absolute } };
            Add(_markerContainer);

            // 3. Playhead
            _playhead = new VisualElement {
                style = {
                    position        = Position.Absolute, width = 1, height = Length.Percent(100),
                    backgroundColor = Color.red
                }
            };
            Add(_playhead);

            // 4. Scrubber Manipulator (背景拖曳)
            this.AddManipulator(new ScrubberManipulator(_controller, this));
        }

        public void SetClip(AnimationClip clip) {
            _clip = clip;
            MarkDirtyRepaint(); // 觸發重繪標尺
        }

        public void SetPlayhead(float time) {
            if (!_clip) return;
            float p = time / _clip.length * 100f;
            _playhead.style.left = Length.Percent(p);
        }

        public void AddMarker(int index, float time, bool isSelected) {
            if (!_clip) return;
            float ratio = time / _clip.length;

            var marker = new MarkerElement(index, isSelected);
            // 設定位置
            marker.style.left = Length.Percent(ratio * 100f);

            // 綁定拖曳邏輯
            marker.AddManipulator(new MarkerManipulator(_controller, index, this));

            _markerContainer.Add(marker);
        }

        public void HighlightMarker(int index) {
            foreach (var child in _markerContainer.Children()) {
                if (child is MarkerElement m) m.SetSelected(m.Index == index);
            }
        }

        public void ClearMarkers() => _markerContainer.Clear();

        public float LocalToTime(float localX) {
            if (!_clip || contentRect.width <= 0) return 0;
            float ratio = Mathf.Clamp01(localX / contentRect.width);

            return ratio * _clip.length;
        }

        // 使用 Painter2D 繪製標尺刻度，效能遠優於創建大量 Label
        private void OnGenerateVisualContent(MeshGenerationContext ctx) {
            if (!_clip) return;

            var   painter  = ctx.painter2D;
            float width    = contentRect.width;
            float height   = contentRect.height;
            float duration = _clip.length;

            if (duration <= 0) return;

            painter.lineWidth   = 1f;
            painter.strokeColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

            // 簡單的自適應刻度邏輯
            float pixelPerSec = width / duration;
            float step        = pixelPerSec > 100 ? 0.1f : (pixelPerSec > 50 ? 0.5f : 1.0f);

            painter.BeginPath();
            for (float t = 0; t <= duration; t += step) {
                float x = (t / duration) * width;
                float h = (Mathf.Abs(t % 1.0f) < 0.001f) ? height : height * 0.3f; // 整秒比較長

                painter.MoveTo(new Vector2(x, 0));
                painter.LineTo(new Vector2(x, h));
            }

            painter.Stroke();
        }
    }

    // ##################################################################
    // [View Component] 關鍵幀圖示
    // ##################################################################
    public class MarkerElement : VisualElement
    {
        public int Index { get; }
        private readonly VisualElement _graphic;

        public MarkerElement(int index, bool selected) {
            Index                = index;
            style.position       = Position.Absolute;
            style.width          = 10;
            style.height         = 15;
            style.marginLeft     = -5; // Center align
            style.justifyContent = Justify.FlexStart;
            style.alignItems     = Align.Center;

            _graphic = new VisualElement {
                style = {
                    width                  = 6, height = 15,
                    backgroundColor        = new Color(0.8f, 0.8f, 0.8f),
                    borderBottomLeftRadius = 2, borderBottomRightRadius = 2
                }
            };
            Add(_graphic);
            SetSelected(selected);
        }

        public void SetSelected(bool selected) {
            _graphic.style.backgroundColor = selected ? new Color(1f, 0.7f, 0.0f) : new Color(0.8f, 0.8f, 0.8f);
        }
    }

    // ##################################################################
    // [View Component] Odin Inspector 整合容器
    // ##################################################################
    public class OdinInspectorView : VisualElement, IDisposable
    {
        private PropertyTree      _tree;
        private EventData         _data;
        private IMGUIContainer    _container;
        private Action<EventData> _onSave;

        public OdinInspectorView(Action<EventData> onSave) {
            _onSave               = onSave;
            style.paddingTop      = 10;
            style.paddingBottom   = 10;
            style.paddingLeft     = 5;
            style.paddingRight    = 5;
            style.backgroundColor = new Color(0.25f, 0.25f, 0.25f);

            _container = new IMGUIContainer(OnGUI) { style = { flexGrow = 1 } };
            Add(_container);
        }

        public void SetData(EventData data, bool forceRefresh = false) {
            if (_data == data && !forceRefresh) return;

            _data = data;

            // 安全釋放舊的 Tree
            _tree?.Dispose();
            _tree = null;

            if (_data != null) {
                _tree         = PropertyTree.Create(_data);
                style.display = DisplayStyle.Flex;
            }
            else {
                style.display = DisplayStyle.None;
            }
        }

        private void OnGUI() {
            if (_tree == null) return;

            _tree.Draw(false);

            // 每次繪製後，如果 Odin 偵測到變更，回調保存
            if (GUI.changed) {
                _onSave?.Invoke(_data);
            }
        }

        public void Dispose() {
            _tree?.Dispose();
        }
    }

    // ##################################################################
    // [Interaction] Manipulators (操作邏輯)
    // ##################################################################

    // 處理時間軸背景的點擊與拖曳 (Scrubbing)
    public class ScrubberManipulator : Manipulator
    {
        private readonly TimelineEditor    _controller;
        private readonly TimelineTrackView _view;

        public ScrubberManipulator(TimelineEditor controller, TimelineTrackView view) {
            _controller = controller;
            _view       = view;
        }

        protected override void RegisterCallbacksOnTarget() {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget() {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnPointerDown(PointerDownEvent evt) {
            if (evt.button != 0) return; // 只回應左鍵

            target.CapturePointer(evt.pointerId); // 關鍵：捕捉滑鼠，防止移出後失效
            Update(evt.localPosition);
            evt.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt) {
            if (target.HasPointerCapture(evt.pointerId)) {
                Update(evt.localPosition);
                evt.StopPropagation();
            }
        }

        private void OnPointerUp(PointerUpEvent evt) {
            if (target.HasPointerCapture(evt.pointerId)) {
                target.ReleasePointer(evt.pointerId);
                evt.StopPropagation();
            }
        }

        private void Update(Vector2 localPos) {
            float time = _view.LocalToTime(localPos.x);
            _controller.SetTime(time);
            _controller.SelectEvent(-1); // 點擊背景取消選取
        }
    }

    // 處理關鍵幀 (Marker) 的拖曳
    public class MarkerManipulator : Manipulator
    {
        private readonly TimelineEditor    _controller;
        private readonly int               _index;
        private readonly TimelineTrackView _trackView;
        private          bool              _isDragging;

        public MarkerManipulator(TimelineEditor controller, int index, TimelineTrackView trackView) {
            _controller = controller;
            _index      = index;
            _trackView  = trackView;
        }

        protected override void RegisterCallbacksOnTarget() {
            target.RegisterCallback<PointerDownEvent>(OnDown);
            target.RegisterCallback<PointerMoveEvent>(OnMove);
            target.RegisterCallback<PointerUpEvent>(OnUp);
        }

        protected override void UnregisterCallbacksFromTarget() {
            target.UnregisterCallback<PointerDownEvent>(OnDown);
            target.UnregisterCallback<PointerMoveEvent>(OnMove);
            target.UnregisterCallback<PointerUpEvent>(OnUp);
        }

        private void OnDown(PointerDownEvent evt) {
            if (evt.button != 0) return;

            _isDragging = true;
            target.CapturePointer(evt.pointerId);
            _controller.SelectEvent(_index); // 選取事件
            evt.StopPropagation();
        }

        private void OnMove(PointerMoveEvent evt) {
            if (_isDragging && target.HasPointerCapture(evt.pointerId)) {
                // 將 Marker 的 Local 座標轉換為 Parent (Track) 的 Local 座標
                Vector2 trackPos = _trackView.WorldToLocal(evt.position);
                float   time     = _trackView.LocalToTime(trackPos.x);

                // 即時預覽時間 (不寫入 Model)
                _controller.SetTime(time);
                evt.StopPropagation();
            }
        }

        private void OnUp(PointerUpEvent evt) {
            if (_isDragging && target.HasPointerCapture(evt.pointerId)) {
                _isDragging = false;
                target.ReleasePointer(evt.pointerId);

                Vector2 trackPos = _trackView.WorldToLocal(evt.position);
                float   time     = _trackView.LocalToTime(trackPos.x);

                // 拖曳結束，寫入 Model
                _controller.MoveEvent(_index, time);
                evt.StopPropagation();
            }
        }
    }
}