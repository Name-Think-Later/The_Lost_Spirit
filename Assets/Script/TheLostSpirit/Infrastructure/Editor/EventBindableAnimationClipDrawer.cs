using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using TheLostSpirit.Infrastructure;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

// ##################################################################
// [工具層] 數學運算
// ##################################################################
static class TimelineMath
{
    public static float FrameToTime(int frame, float rate) {
        return rate <= 0 ? 0 : frame / rate;
    }

    public static int TimeToFrame(float time, float rate) {
        return rate <= 0 ? 0 : Mathf.RoundToInt(time * rate);
    }

    public static float SnapTime(float time, float rate) {
        if (rate <= 0) {
            return time;
        }

        return Mathf.Round(time * rate) / rate;
    }

    public static float ClampTime(float time, float max) {
        return Mathf.Clamp(time, 0, max);
    }
}

// ##################################################################
// [工具層] 資料序列化
// ##################################################################
static class EventDataSerializer
{
    public static EventData Deserialize(string dataStr) {
        if (string.IsNullOrEmpty(dataStr)) {
            return new EventData();
        }

        try {
            var bytes = Convert.FromBase64String(dataStr);

            return SerializationUtility.DeserializeValue<EventData>(bytes, DataFormat.Binary);
        }
        catch {
            try {
                return JsonUtility.FromJson<EventData>(dataStr);
            }
            catch {
                return new EventData();
            }
        }
    }

    public static string Serialize(EventData data) {
        if (data == null) {
            return "";
        }

        var bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);

        return Convert.ToBase64String(bytes);
    }
}

// ##################################################################
// [工具層] UI 建構工廠
// ##################################################################
static class UIBuilder
{
    public static VisualElement CreateContainer(float height, Color bg, Color border) {
        var el = new VisualElement {
            style = {
                marginTop      = 2, marginBottom           = 5, backgroundColor      = bg,
                borderTopWidth = 1, borderBottomWidth      = 1, borderLeftWidth      = 1, borderRightWidth      = 1,
                borderTopColor = border, borderBottomColor = border, borderLeftColor = border, borderRightColor = border
            }
        };
        if (height > 0) {
            el.style.height = height;
        }

        return el;
    }

    public static VisualElement CreateAbsoluteLayer(Color bg, float top = 0, float height = -1) {
        var el = new VisualElement
            { style = { position = Position.Absolute, left = 0, right = 0, top = top, backgroundColor = bg } };
        if (height > 0) {
            el.style.height = height;
        }
        else {
            el.style.bottom = 0;
        }

        return el;
    }

    public static Button CreateIconButton(
        Action onClick,
        string iconName,
        string tooltip,
        Color  normalColor,
        Color  hoverColor,
        Color  border
    ) {
        var icon = EditorGUIUtility.IconContent(iconName).image;
        var btn = new Button(onClick) {
            tooltip = tooltip,
            style = {
                width               = 24, height                       = 20, marginLeft = 2,
                backgroundImage     = (Texture2D)icon, backgroundColor = normalColor,
                borderTopWidth      = 1, borderBottomWidth             = 1, borderLeftWidth = 1, borderRightWidth = 1,
                borderTopColor      = border, borderBottomColor        = border, borderLeftColor = border,
                borderRightColor    = border,
                backgroundSize      = new BackgroundSize(BackgroundSizeType.Contain),
                backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center),
                backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center)
            }
        };
        btn.RegisterCallback<MouseEnterEvent>(_ => btn.style.backgroundColor = hoverColor);
        btn.RegisterCallback<MouseLeaveEvent>(_ => btn.style.backgroundColor = normalColor);

        return btn;
    }
}

// ##################################################################
// [核心] Property Drawer 入口
// ##################################################################
[CustomPropertyDrawer(typeof(EventBindableAnimationClip))]
public class EventBindableAnimationClipDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        var root = new VisualElement();

        // 繪製預設屬性 (targetClip)
        var iterator    = property.Copy();
        var endProperty = iterator.GetEndProperty();
        if (iterator.NextVisible(true)) {
            do {
                if (SerializedProperty.EqualContents(iterator, endProperty)) {
                    break;
                }

                var propField = new PropertyField(iterator.Copy());
                if (iterator.name == nameof(EventBindableAnimationClip.targetClip)) {
                    propField.name = "clip-field";
                }

                propField.style.marginBottom = 2;
                root.Add(propField);
            } while (iterator.NextVisible(false));
        }

        // 初始化 Timeline 控制器
        var controller = new TimelineController(root, property);
        controller.Initialize();

        return root;
    }

    // ##################################################################
    // [控制器] Timeline Controller (The Brain)
    // ##################################################################
    public class TimelineController
    {
        public TimelineController(VisualElement root, SerializedProperty property) {
            _root         = root;
            _rootProperty = property;
            _clipProperty = property.FindPropertyRelative(nameof(EventBindableAnimationClip.targetClip));
        }

        public void Initialize() {
            BuildUI();
            BindEvents();
            _root.schedule.Execute(() => FullRefresh());
        }

        #region [Configuration] Constants & Styles

        static class Layout
        {
            public const float TrackHeight   = 45f;
            public const float ClipBarHeight = 20f;
            public const float RulerTop      = 20f;
            public const float MarkerTop     = -1f;
            public const float MarkerWidth   = 4f;
            public const float MarkerHeight  = 15f;
            public const float DragThreshold = 5f;
        }

        static class Styles
        {
            public static readonly Color TrackBg        = new Color(0.15f, 0.15f, 0.15f);
            public static readonly Color ClipBar        = new Color(0.3f, 0.4f, 0.55f);
            public static readonly Color MarkerNormal   = new Color(0.7f, 0.7f, 0.7f);
            public static readonly Color MarkerSelected = new Color(1f, 0.7f, 0.2f);
            public static readonly Color DurationBar    = new Color(0.4f, 0.8f, 0.4f, 0.4f);
            public static readonly Color Border         = new Color(0.1f, 0.1f, 0.1f);
            public static readonly Color InspectorBg    = new Color(0.2f, 0.2f, 0.2f);
            public static readonly Color InspectorBox   = new Color(0.25f, 0.25f, 0.25f);
            public static readonly Color ButtonNormal   = new Color(0.22f, 0.22f, 0.22f);
            public static readonly Color ButtonHover    = new Color(0.35f, 0.35f, 0.35f);
            public static readonly Color ButtonDanger   = new Color(0.6f, 0.2f, 0.2f);
            public static readonly Color RulerLine      = Color.gray;
            public static readonly Color RulerText      = Color.gray;
        }

        const string FIXED_EVENT_NAME = "AnimationEventCallBack";

        #endregion

        #region [1. Data & State]

        readonly VisualElement      _root;
        readonly SerializedProperty _rootProperty;
        readonly SerializedProperty _clipProperty;

        AnimationClip _currentClip;
        GameObject    _targetGameObject;
        EventData     _currentEventData;
        PropertyTree  _odinTree;

        // Timeline State
        float _currentTime;
        int   _selectedEventIndex = -1;
        float _lastCachedLength   = -1f;
        bool  _isInternalChange;

        int           _dragIndex;
        VisualElement _dragTarget;

        // UI References
        VisualElement _header,      _trackRoot,        _timeContainer, _inspectorContainer;
        VisualElement _markerLayer, _cursorLayer,      _playHeadLine,  _clipBar, _rulerLayer, _durationBar;
        Label         _timeLabel,   _totalFramesLabel, _noClipLabel,   _inspectorTimeLabel;
        IntegerField  _frameField,  _inspectorFrameField;
        Button        _btnAddEvent;

        #endregion

        #region [2. Initialization & Binding]

        void BuildUI() {
            BuildHeader();

            BuildTracker();

            BuildInspector();
        }

        void BuildInspector() {
            _inspectorContainer                     = UIBuilder.CreateContainer(-1, Styles.InspectorBg, Styles.Border);
            _inspectorContainer.style.paddingTop    = 5;
            _inspectorContainer.style.paddingBottom = 5;
            _inspectorContainer.style.paddingLeft   = 5;
            _inspectorContainer.style.paddingRight  = 5;
            _inspectorContainer.style.display       = DisplayStyle.None;
            _root.Add(_inspectorContainer);
        }

        void BuildTracker() {
            _trackRoot = UIBuilder.CreateContainer(Layout.TrackHeight, Styles.TrackBg, Styles.Border);
            _trackRoot.style.overflow = Overflow.Visible;
            _trackRoot.style.justifyContent = Justify.Center;
            _trackRoot.style.alignItems = Align.Center;

            _noClipLabel = new Label("No Clip Assigned") {
                style = { color = Color.gray, unityFontStyleAndWeight = FontStyle.Italic, alignSelf = Align.Center }
            };

            _clipBar = UIBuilder.CreateAbsoluteLayer(Styles.ClipBar, 0, Layout.ClipBarHeight);

            _rulerLayer = UIBuilder.CreateAbsoluteLayer(Color.clear, Layout.RulerTop);

            _durationBar = UIBuilder.CreateAbsoluteLayer(Styles.DurationBar, 0, Layout.ClipBarHeight);
            _durationBar.style.display = DisplayStyle.None;
            _durationBar.style.borderRightWidth = 1;
            _durationBar.style.borderRightColor = new Color(1, 1, 1, 0.5f);
            _durationBar.pickingMode = PickingMode.Ignore;

            _markerLayer = UIBuilder.CreateAbsoluteLayer(Color.clear);

            _cursorLayer             = UIBuilder.CreateAbsoluteLayer(Color.clear);
            _cursorLayer.pickingMode = PickingMode.Ignore;

            _playHeadLine = new VisualElement {
                style = {
                    position   = Position.Absolute, width = 1, top = 0, bottom = 0, backgroundColor = Color.white,
                    marginLeft = -0.5f
                }
            };

            _cursorLayer.Add(_playHeadLine);
            _trackRoot.Add(_clipBar);
            _trackRoot.Add(_rulerLayer);
            _trackRoot.Add(_durationBar);
            _trackRoot.Add(_markerLayer);
            _trackRoot.Add(_cursorLayer);
            _root.Add(_trackRoot);
        }

        void BuildHeader() {
            _header = new VisualElement {
                style = {
                    flexDirection = FlexDirection.Row, marginTop = 5, justifyContent = Justify.SpaceBetween,
                    alignItems    = Align.Center
                }
            };
            _btnAddEvent =
                UIBuilder.CreateIconButton(
                    OnAddEventClicked,
                    "Animation.AddEvent",
                    "Add Event",
                    Styles.ButtonNormal,
                    Styles.ButtonHover,
                    Styles.Border
                );

            _timeContainer = new VisualElement
                { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } };
            _timeLabel = new Label("0.00s (")
                { style = { minWidth = 40, unityTextAlign = TextAnchor.MiddleRight, marginRight = 5 } };
            _frameField = new IntegerField
                { style = { width = 45, height = 18, marginLeft = 2, marginRight = 2 }, isDelayed = true };
            _totalFramesLabel = new Label("/ 0 f )");

            _timeContainer.Add(_timeLabel);
            _timeContainer.Add(_frameField);
            _timeContainer.Add(_totalFramesLabel);
            _header.Add(_btnAddEvent);
            _header.Add(_timeContainer);
            _root.Add(_header);
        }

        void BindEvents() {
            _root
                .Q<PropertyField>("clip-field")
                ?.RegisterValueChangeCallback(_ => {
                    _rootProperty.serializedObject.ApplyModifiedProperties();
                    FullRefresh();
                });

            _frameField.RegisterValueChangedCallback(OnPlayHeadFrameChanged);

            _trackRoot.RegisterCallback<PointerDownEvent>(OnTrackPointerDown);
            _trackRoot.RegisterCallback<PointerMoveEvent>(OnTrackPointerMove);
            _trackRoot.RegisterCallback<PointerUpEvent>(OnTrackPointerUp);

            _root.RegisterCallback<AttachToPanelEvent>(OnAttach);
            _root.RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }

        void OnAttach(AttachToPanelEvent evt) {
            Undo.undoRedoPerformed              += OnUndoRedo;
            AnimationUtility.onCurveWasModified += OnClipModified;
            EditorApplication.update            += OnEditorUpdate;
        }

        void OnDetach(DetachFromPanelEvent evt) {
            Undo.undoRedoPerformed              -= OnUndoRedo;
            AnimationUtility.onCurveWasModified -= OnClipModified;
            EditorApplication.update            -= OnEditorUpdate;
            CleanupResources();
        }

        void CleanupResources() {
            CleanupOdinResources();
            if (AnimationMode.InAnimationMode()) {
                AnimationMode.StopAnimationMode();
            }
        }

        void CleanupOdinResources() {
            if (_odinTree != null) {
                _odinTree.Dispose();
                _odinTree = null;
            }

            _currentEventData = null;
        }

        #endregion

        #region [3. Logic & Operations]

        void OnUndoRedo() {
            FullRefresh(true);
        }

        void OnClipModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType type) {
            if (_isInternalChange) {
                _isInternalChange = false;

                return;
            }

            if (clip == _currentClip) {
                FullRefresh(true);
            }
        }

        void OnEditorUpdate() {
            if (!_currentClip) {
                return;
            }

            if (Mathf.Approximately(_currentClip.length, _lastCachedLength)) {
                return;
            }

            if (_currentTime >= _lastCachedLength - 0.01f || _currentTime > _currentClip.length) {
                _currentTime = _currentClip.length;
            }

            FullRefresh(true);
        }

        void OnPlayHeadFrameChanged(ChangeEvent<int> evt) {
            _currentTime = TimelineMath.FrameToTime(evt.newValue, _currentClip.frameRate);
            _currentTime = TimelineMath.ClampTime(_currentTime, _currentClip.length);
            UpdateCursorVisual();
            SampleAnimation();
        }

        void OnAddEventClicked() {
            AddEvent(_currentTime);
        }

        void AddEvent(float time) {
            var frame = TimelineMath.TimeToFrame(time, _currentClip.frameRate);
            if (IsFrameOccupied(frame)) {
                Debug.LogWarning($"Frame {frame} is already occupied.");

                return;
            }

            var events   = AnimationUtility.GetAnimationEvents(_currentClip).ToList();
            var newEvent = new AnimationEvent { time = time, functionName = FIXED_EVENT_NAME, stringParameter = "" };
            events.Add(newEvent);

            var sortedEvents = events.OrderBy(e => e.time).ToArray();
            CommitTransaction(sortedEvents, "Add Animation Event");

            var targetIndex = FindEventIndex(sortedEvents, time);
            _selectedEventIndex = targetIndex != -1 ? targetIndex : events.Count - 1;

            FullRefresh(true);
        }

        void DeleteEvent(int index) {
            var events = AnimationUtility.GetAnimationEvents(_currentClip).ToList();

            if (index < 0 || index >= events.Count) {
                return;
            }

            // [Safety] Double-Tap Deletion to ensure data is wiped from Undo history
            var evtToKill = events[index];
            evtToKill.stringParameter = "";
            events[index]             = evtToKill; // Write empty back
            events.RemoveAt(index);                // Then remove

            CommitTransaction(events.ToArray(), "Delete Animation Event");

            _selectedEventIndex = -1;

            CleanupOdinResources();
            FullRefresh(true);
        }

        void ChangeSelection(int newIndex) {
            var events = AnimationUtility.GetAnimationEvents(_currentClip);

            if (newIndex < 0 || newIndex >= events.Length) {
                return;
            }

            _selectedEventIndex = newIndex;

            UpdateMarkerSelectionVisuals();
            DrawEventInspector();
        }

        void OnEventFrameModified(int newFrame) {
            if (_selectedEventIndex < 0) {
                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip);

            if (_selectedEventIndex >= events.Length) {
                return;
            }

            if (IsFrameOccupied(newFrame, _selectedEventIndex)) {
                Debug.LogWarning($"Frame {newFrame} occupied.");
                // Revert visual
                var currentFrame = TimelineMath.TimeToFrame(events[_selectedEventIndex].time, _currentClip.frameRate);
                _inspectorFrameField.SetValueWithoutNotify(currentFrame);

                return;
            }

            var newTime = TimelineMath.FrameToTime(newFrame, _currentClip.frameRate);
            newTime = TimelineMath.ClampTime(newTime, _currentClip.length);

            events[_selectedEventIndex].time = newTime;
            var sortedEvents = events.OrderBy(e => e.time).ToArray();

            CommitTransaction(sortedEvents, "Move Animation Event");

            var newIndex = FindEventIndex(sortedEvents, newTime);
            if (newIndex != -1) {
                _selectedEventIndex = newIndex;
            }

            FullRefresh(true);
        }

        // [Helper] Centralize transaction commit logic
        void CommitTransaction(AnimationEvent[] events, string undoName) {
            _isInternalChange = true; // Prevent loop
            Undo.RecordObject(_currentClip, undoName);
            AnimationUtility.SetAnimationEvents(_currentClip, events);
            EditorUtility.SetDirty(_currentClip); // Force Save
        }

        bool IsFrameOccupied(int targetFrame, int excludeIndex = -1) {
            var events = AnimationUtility.GetAnimationEvents(_currentClip);

            return events
                   .Where((t, i) => i != excludeIndex)
                   .Any(t => TimelineMath.TimeToFrame(t.time, _currentClip.frameRate) == targetFrame);
        }

        int FindEventIndex(AnimationEvent[] events, float targetTime) {
            for (var i = 0; i < events.Length; i++)
                if (Mathf.Approximately(events[i].time, targetTime)) {
                    return i;
                }

            return -1;
        }

        // [New] Centralized Save Logic for Odin
        void SaveEventData() {
            var evt = Event.current;

            if (!GUI.changed && evt.type is EventType.Repaint or EventType.Layout) return;

            var base64    = EventDataSerializer.Serialize(_currentEventData);
            var allEvents = AnimationUtility.GetAnimationEvents(_currentClip);

            allEvents[_selectedEventIndex].stringParameter = base64;
            CommitTransaction(allEvents, "Modify Event Data");
        }

        #endregion

        #region [4. View & Rendering]

        void FullRefresh(bool preserveTime = false) {
            var newClip = _clipProperty.objectReferenceValue as AnimationClip;
            if (newClip != _currentClip) {
                CleanupResources();
                _currentClip        = newClip;
                _currentTime        = 0;
                _selectedEventIndex = -1;
            }

            _lastCachedLength = _currentClip ? _currentClip.length : 0f;

            _rulerLayer.Clear();
            _markerLayer.Clear();
            _trackRoot.style.display = DisplayStyle.Flex;
            _header.style.display    = DisplayStyle.Flex;

            if (!preserveTime) {
                _currentTime    = 0;
                _timeLabel.text = "0.00s";
                _frameField.SetValueWithoutNotify(0);
            }

            if (_currentClip && _selectedEventIndex >= AnimationUtility.GetAnimationEvents(_currentClip).Length) {
                _selectedEventIndex = -1;
            }


            if (!_currentClip) {
                SetLayersVisible(false);
                if (!_trackRoot.Contains(_noClipLabel)) {
                    _trackRoot.Add(_noClipLabel);
                }

                _inspectorContainer.style.display = DisplayStyle.None;
                _btnAddEvent.style.display        = DisplayStyle.None;
                _timeContainer.style.display      = DisplayStyle.None;
            }
            else {
                SetLayersVisible(true);
                if (_trackRoot.Contains(_noClipLabel)) {
                    _trackRoot.Remove(_noClipLabel);
                }

                _btnAddEvent.style.display   = DisplayStyle.Flex;
                _timeContainer.style.display = DisplayStyle.Flex;

                DrawRuler();
                RefreshMarkers();
                UpdateCursorVisual();
                UpdateDurationVisuals();

                SampleAnimation();
                DrawEventInspector();
            }
        }

        void SetLayersVisible(bool visible) {
            var display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            for (var i = 0; i < _trackRoot.childCount; i++)
                if (_trackRoot[i] != _noClipLabel) {
                    _trackRoot[i].style.display = display;
                }
        }

        void UpdateDurationVisuals() {
            _durationBar.style.display = DisplayStyle.None;
            if (_selectedEventIndex == -1 || _currentEventData == null) {
                return;
            }

            if (_currentEventData.CantInspectDuration) return;
            var selectedIdx = _currentEventData.SelectedIndex;
            var action      = _currentEventData._actions[selectedIdx];

            var frames          = action.DurationFrames;
            var durationSeconds = TimelineMath.FrameToTime(frames, _currentClip.frameRate);
            var events          = AnimationUtility.GetAnimationEvents(_currentClip);

            var startTime  = events[_selectedEventIndex].time;
            var startRatio = startTime / _currentClip.length;
            var widthRatio = durationSeconds / _currentClip.length;

            _durationBar.style.left    = Length.Percent(startRatio * 100f);
            _durationBar.style.width   = Length.Percent(widthRatio * 100f);
            _durationBar.style.display = DisplayStyle.Flex;
            _durationBar.tooltip       = $"{frames} frames ({durationSeconds:F2}s)";
        }

        void DrawOdinArea(AnimationEvent evt) {
            var currentJson = evt.stringParameter;
            _currentEventData = EventDataSerializer.Deserialize(currentJson);

            _odinTree?.Dispose();
            _odinTree = PropertyTree.Create(_currentEventData);
            _currentEventData.ResetSelection();

            var odinContainer = new IMGUIContainer(() => {
                _odinTree.BeginDraw(false);
                _odinTree.Draw(false);
                _odinTree.EndDraw();
                _odinTree.ApplyChanges();

                SaveEventData();
                UpdateDurationVisuals();
            });

            var box = UIBuilder.CreateContainer(-1, Styles.InspectorBox, Color.clear);
            box.style.paddingTop    = 5;
            box.style.paddingBottom = 5;
            box.style.paddingLeft   = 5;
            box.style.paddingRight  = 5;
            box.Add(odinContainer);
            _inspectorContainer.Add(box);
        }

        void DrawRuler() {
            var duration = _currentClip.length;
            var step     = duration < 2f ? 0.1f : duration < 4f ? 0.5f : 1f;
            var count    = Mathf.CeilToInt(duration / step);
            if (count > 200) {
                count = 200;
            }

            for (var i = 0; i <= count; i++) {
                var t = i * step;

                if (t > duration) {
                    break;
                }

                var p = t / duration * 100f;
                _rulerLayer.Add(new VisualElement {
                    style = {
                        position        = Position.Absolute, left = Length.Percent(p), top = 0, width = 1, height = 6,
                        backgroundColor = Styles.RulerLine
                    }
                });
                _rulerLayer.Add(new Label(t.ToString("0.0")) {
                    style = {
                        position = Position.Absolute, left = Length.Percent(p), top = 10, fontSize = 9,
                        color    = Styles.RulerText
                    }
                });
            }
        }

        void RefreshMarkers() {
            _markerLayer.Clear();

            var events = AnimationUtility.GetAnimationEvents(_currentClip);
            for (var i = 0; i < events.Length; i++) {
                var evt        = events[i];
                var index      = i;
                var p          = evt.time / _currentClip.length * 100f;
                var isSelected = index == _selectedEventIndex;

                var marker = new VisualElement {
                    style = {
                        position         = Position.Absolute, left = Length.Percent(p), top = Layout.MarkerTop,
                        width            = Layout.MarkerWidth, height = Layout.MarkerHeight,
                        backgroundColor  = isSelected ? Styles.MarkerSelected : Styles.MarkerNormal,
                        borderTopWidth   = 1, borderBottomWidth = 1, borderLeftWidth = 1, borderRightWidth = 1,
                        borderTopColor   = Color.black, borderBottomColor = Color.black, borderLeftColor = Color.black,
                        borderRightColor = Color.black,
                        marginLeft       = -Layout.MarkerWidth / 2f
                    },
                    tooltip = $"{evt.functionName}\n({evt.time:F2}s)"
                };

                marker.RegisterCallback<PointerDownEvent>(e => OnMarkerPointerDown(e, marker, index));
                marker.RegisterCallback<PointerMoveEvent>(OnMarkerPointerMove);
                marker.RegisterCallback<PointerUpEvent>(OnMarkerPointerUp);
                marker.RegisterCallback<MouseDownEvent>(e => e.StopPropagation());
                _markerLayer.Add(marker);
            }
        }

        void UpdateMarkerSelectionVisuals() {
            for (var i = 0; i < _markerLayer.childCount; i++) {
                var marker = _markerLayer.ElementAt(i);
                marker.style.backgroundColor = i == _selectedEventIndex ? Styles.MarkerSelected : Styles.MarkerNormal;
            }
        }

        void UpdateCursorVisual() {
            var p = _currentTime / _currentClip.length * 100f;
            _playHeadLine.style.left = Length.Percent(p);

            var frame = TimelineMath.TimeToFrame(_currentTime, _currentClip.frameRate);
            var total = TimelineMath.TimeToFrame(_currentClip.length, _currentClip.frameRate);

            _timeLabel.text = $"{_currentTime:F2}s";
            _frameField.SetValueWithoutNotify(frame);
            _totalFramesLabel.text = $"/ {total} f )";
        }

        void SampleAnimation() {
            if (!_targetGameObject) {
                _targetGameObject =
                    _rootProperty.serializedObject.targetObject switch {
                        Component c  => c.gameObject,
                        GameObject g => g,
                        _            => null
                    };
            }
            else {
                if (!AnimationMode.InAnimationMode()) {
                    AnimationMode.StartAnimationMode();
                }

                AnimationMode.SampleAnimationClip(_targetGameObject, _currentClip, _currentTime);
                SceneView.RepaintAll();
            }
        }

        void DrawEventInspector() {
            _inspectorContainer.Clear();
            CleanupOdinResources();

            if (_selectedEventIndex == -1) {
                _inspectorContainer.style.display = DisplayStyle.None;

                return;
            }

            var events = AnimationUtility.GetAnimationEvents(_currentClip);
            if (_selectedEventIndex >= events.Length) {
                _selectedEventIndex               = -1;
                _inspectorContainer.style.display = DisplayStyle.None;

                return;
            }

            _inspectorContainer.style.display = DisplayStyle.Flex;
            var evt = events[_selectedEventIndex];

            // Nav
            var navContainer = new VisualElement {
                style = {
                    flexDirection = FlexDirection.Row, justifyContent = Justify.SpaceBetween, alignItems = Align.Center,
                    marginBottom  = 5
                }
            };

            var btnPrev = new Button(() => ChangeSelection(_selectedEventIndex - 1))
                { text = "◄", style = { width = 30 } };
            btnPrev.SetEnabled(_selectedEventIndex > 0);

            var centerGroup = new VisualElement
                { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } };

            _inspectorTimeLabel = new Label($"{evt.time:F2}s  /")
                { style = { marginRight = 5, unityFontStyleAndWeight = FontStyle.Bold } };

            var currentFrame = TimelineMath.TimeToFrame(evt.time, _currentClip.frameRate);
            _inspectorFrameField = new IntegerField { value = currentFrame, isDelayed = true, style = { width = 50 } };

            _inspectorFrameField.RegisterValueChangedCallback(e => OnEventFrameModified(e.newValue));

            var btnNext = new Button(() => ChangeSelection(_selectedEventIndex + 1))
                { text = "►", style = { width = 30 } };
            btnNext.SetEnabled(_selectedEventIndex < events.Length - 1);

            var footer = new VisualElement
                { style = { flexDirection = FlexDirection.Row, justifyContent = Justify.FlexEnd, marginTop = 5 } };
            var btnDelete = new Button(() => DeleteEvent(_selectedEventIndex)) {
                text = "Delete", style = { width = 80, backgroundColor = Styles.ButtonDanger, color = Color.white }
            };

            centerGroup.Add(_inspectorTimeLabel);
            centerGroup.Add(_inspectorFrameField);
            centerGroup.Add(new Label(" f"));
            navContainer.Add(btnPrev);
            navContainer.Add(centerGroup);
            navContainer.Add(btnNext);
            footer.Add(btnDelete);
            _inspectorContainer.Add(navContainer);
            _inspectorContainer.Add(UIBuilder.CreateContainer(1, Styles.Border, Color.clear));
            DrawOdinArea(evt);
            _inspectorContainer.Add(UIBuilder.CreateContainer(1, Styles.Border, Color.clear));
            _inspectorContainer.Add(footer);
        }

        #endregion

        #region [Interaction] Mouse & Input

        void OnTrackPointerDown(PointerDownEvent evt) {
            if (!_currentClip) return;

            if (!AnimationMode.InAnimationMode()) {
                AnimationMode.StartAnimationMode();
            }

            _trackRoot.CapturePointer(evt.pointerId);
            Scrub(evt.localPosition.x);
        }

        void OnTrackPointerMove(PointerMoveEvent evt) {
            if (!_trackRoot.HasPointerCapture(evt.pointerId)) return;

            Scrub(evt.localPosition.x);
        }

        void OnTrackPointerUp(PointerUpEvent evt) {
            _trackRoot.ReleasePointer(evt.pointerId);
        }

        void Scrub(float localX) {
            var rawTime =
                TimelineMath.ClampTime(localX / _trackRoot.resolvedStyle.width * _currentClip.length,
                                       _currentClip.length);

            _currentTime = TimelineMath.SnapTime(rawTime, _currentClip.frameRate);
            UpdateCursorVisual();
            SampleAnimation();
        }

        void OnMarkerPointerDown(PointerDownEvent evt, VisualElement target, int index) {
            if (evt.button != 0) return;

            _dragTarget = target;
            _dragIndex  = index;

            if (_selectedEventIndex != index) {
                _selectedEventIndex = index;
                UpdateMarkerSelectionVisuals();
                DrawEventInspector();
            }

            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        void OnMarkerPointerMove(PointerMoveEvent evt) {
            if (!_dragTarget.HasPointerCapture(evt.pointerId)) return;

            var width = _markerLayer.resolvedStyle.width;
            var rawTime =
                TimelineMath.ClampTime(_markerLayer.WorldToLocal(evt.position).x / width * _currentClip.length,
                                       _currentClip.length);

            var snappedTime  = TimelineMath.SnapTime(rawTime, _currentClip.frameRate);
            var snappedRatio = snappedTime / _currentClip.length;

            _dragTarget.style.left = Length.Percent(snappedRatio * 100f);

            var frame = TimelineMath.TimeToFrame(snappedTime, _currentClip.frameRate);

            _inspectorFrameField.SetValueWithoutNotify(frame);
            _inspectorTimeLabel.text = $"{snappedTime:F2}s  /";
            _dragTarget.tooltip      = $"Dragging...\n({snappedTime:F2}s | f: {frame})";
            evt.StopPropagation();
        }

        void OnMarkerPointerUp(PointerUpEvent evt) {
            _dragTarget.ReleasePointer(evt.pointerId);

            var width = _markerLayer.resolvedStyle.width;
            var rawTime =
                TimelineMath.ClampTime(_markerLayer.WorldToLocal(evt.position).x / width * _currentClip.length,
                                       _currentClip.length);

            var finalTime  = TimelineMath.SnapTime(rawTime, _currentClip.frameRate);
            var finalFrame = TimelineMath.TimeToFrame(finalTime, _currentClip.frameRate);

            if (IsFrameOccupied(finalFrame, _dragIndex)) {
                Debug.LogWarning($"Frame {finalFrame} is already occupied. Reverting.");
                RefreshMarkers();

                var currentEvents = AnimationUtility.GetAnimationEvents(_currentClip);
                if (_dragIndex >= 0 && _dragIndex < currentEvents.Length) {
                    var originalTime  = currentEvents[_dragIndex].time;
                    var originalFrame = TimelineMath.TimeToFrame(originalTime, _currentClip.frameRate);

                    _inspectorFrameField.SetValueWithoutNotify(originalFrame);
                    _inspectorTimeLabel.text = $"{originalTime:F2}s  /";
                }
            }
            else {
                var events = AnimationUtility.GetAnimationEvents(_currentClip);
                if (_dragIndex >= 0 && _dragIndex < events.Length) {
                    events[_dragIndex].time = finalTime;
                    CommitTransaction(events, "Move Animation Event");
                }

                var newEvents = AnimationUtility.GetAnimationEvents(_currentClip);
                var newIndex  = FindEventIndex(newEvents, finalTime);
                if (newIndex != -1) {
                    _selectedEventIndex = newIndex;
                }

                RefreshMarkers();
            }

            _dragTarget = null;
            evt.StopPropagation();
        }

        #endregion
    }
}