using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
// 用於 PropertyField

// 綁定到 TimelineTrigger 腳本
[CustomEditor(typeof(TimelineTrigger))]
public class TimelineTriggerEditor : Editor
{
    VisualElement _clipBar;
    float         _currentTime;
    VisualElement _playhead;

    // UI 參考
    VisualElement   _root;
    VisualElement   _rulerContainer;
    TimelineTrigger _targetScript; // 當前的腳本實例
    Label           _timeLabel;
    VisualElement   _trackContainer;

    // 這是 UI Toolkit 取代 OnInspectorGUI 的入口
    public override VisualElement CreateInspectorGUI() {
        // 1. 獲取當前編輯的腳本
        _targetScript = (TimelineTrigger)target;

        _root = new VisualElement();

        // 2. 繪製預設 Inspector (讓原本的 targetClip 欄位顯示出來)
        // InspectorElement.FillDefaultInspector(root, serializedObject, this); 
        // 或者手動畫出 PropertyField 以便監聽變更：
        var clipProp  = serializedObject.FindProperty("targetClip");
        var clipField = new PropertyField(clipProp, "Animation Clip");

        // 當使用者在 Inspector 更換 Clip 時，重繪時間軸
        clipField.RegisterValueChangeCallback(evt => {
            serializedObject.ApplyModifiedProperties(); // 確保數據已寫入
            RebuildTimeline();
        });
        _root.Add(clipField);

        // 3. 加入時間軸標題
        var header = new VisualElement
            { style = { flexDirection = FlexDirection.Row, marginTop = 10, justifyContent = Justify.SpaceBetween } };
        header.Add(new Label("Event Timeline Editor") { style = { unityFontStyleAndWeight = FontStyle.Bold } });
        _timeLabel = new Label("Time: 0.00s");
        header.Add(_timeLabel);
        _root.Add(header);

        // 4. 建立軌道容器 (Container)
        _trackContainer                         = new VisualElement();
        _trackContainer.style.height            = 80;
        _trackContainer.style.marginTop         = 5;
        _trackContainer.style.backgroundColor   = new Color(0.15f, 0.15f, 0.15f);
        _trackContainer.style.borderTopWidth    = 1;
        _trackContainer.style.borderBottomWidth = 1;
        _trackContainer.style.borderLeftWidth   = 1;
        _trackContainer.style.borderRightWidth  = 1;

        _trackContainer.style.borderTopColor    = Color.black;
        _trackContainer.style.borderBottomColor = Color.black;
        _trackContainer.style.borderLeftColor   = Color.black;
        _trackContainer.style.borderRightColor  = Color.black;
        _trackContainer.style.overflow          = Overflow.Hidden; // 確保內容不超出去

        _root.Add(_trackContainer);

        // 5. 初始化事件與繪製
        SetupInputEvents();

        // 為了確保開啟 Inspector 時有畫面，稍後執行一次 Rebuild
        _root.schedule.Execute(RebuildTimeline);

        return _root;
    }

    void RebuildTimeline() {
        _trackContainer.Clear();

        // 從腳本中讀取 Clip
        var clip = _targetScript.targetClip;

        if (clip == null) {
            var warning = new Label("Please assign an Animation Clip above.") {
                style = { color = Color.gray, alignSelf = Align.Center, top = 30 }
            };
            _trackContainer.Add(warning);

            return;
        }

        // A. 繪製藍色 Clip 條
        _clipBar                         = new VisualElement();
        _clipBar.style.position          = Position.Absolute;
        _clipBar.style.left              = 0;
        _clipBar.style.right             = 0;
        _clipBar.style.top               = 0;
        _clipBar.style.height            = 40;
        _clipBar.style.backgroundColor   = new Color(0.23f, 0.35f, 0.6f);
        _clipBar.style.borderBottomWidth = 1;
        _clipBar.style.borderBottomColor = Color.black;
        _trackContainer.Add(_clipBar);

        // B. 繪製刻度
        _rulerContainer                = new VisualElement();
        _rulerContainer.style.position = Position.Absolute;
        _rulerContainer.style.top      = 40;
        _rulerContainer.style.bottom   = 0;
        _rulerContainer.style.left     = 0;
        _rulerContainer.style.right    = 0;
        _trackContainer.Add(_rulerContainer);
        DrawRuler(clip.length);

        // C. 繪製事件
        DrawEvents(clip);

        // D. 繪製播放頭
        _playhead                       = new VisualElement();
        _playhead.style.position        = Position.Absolute;
        _playhead.style.width           = 1;
        _playhead.style.top             = 0;
        _playhead.style.bottom          = 0;
        _playhead.style.backgroundColor = Color.white;

        var headIcon = new VisualElement();
        headIcon.style.width                   = 10;
        headIcon.style.height                  = 10;
        headIcon.style.backgroundColor         = Color.white;
        headIcon.style.marginLeft              = -5;
        headIcon.style.borderBottomLeftRadius  = 5;
        headIcon.style.borderBottomRightRadius = 5;
        _playhead.Add(headIcon);

        _trackContainer.Add(_playhead);
        UpdatePlayheadVisual();
    }

    void DrawRuler(float duration) {
        var seconds = Mathf.CeilToInt(duration);
        for (var i = 0; i <= seconds; i++) {
            var percent = i / duration * 100f;
            if (percent > 100) {
                break;
            }

            var tick = new VisualElement();
            tick.style.position        = Position.Absolute;
            tick.style.left            = Length.Percent(percent);
            tick.style.top             = 0;
            tick.style.width           = 1;
            tick.style.height          = 10;
            tick.style.backgroundColor = Color.gray;
            _rulerContainer.Add(tick);

            var label = new Label(i.ToString());
            label.style.position = Position.Absolute;
            label.style.left     = Length.Percent(percent);
            label.style.top      = 10;
            label.style.fontSize = 10;
            label.style.color    = Color.gray;
            _rulerContainer.Add(label);
        }
    }

    void DrawEvents(AnimationClip clip) {
        var events = AnimationUtility.GetAnimationEvents(clip);
        foreach (var evt in events) {
            var percent = evt.time / clip.length * 100f;
            var marker  = new VisualElement();
            marker.style.position          = Position.Absolute;
            marker.style.left              = Length.Percent(percent);
            marker.style.top               = 2;
            marker.style.width             = 4;
            marker.style.height            = 20;
            marker.style.backgroundColor   = new Color(1f, 0.8f, 0.2f);
            marker.style.borderTopWidth    = 1;
            marker.style.borderBottomWidth = 1;
            marker.style.borderLeftWidth   = 1;
            marker.style.borderRightWidth  = 1;

            marker.style.borderTopColor    = Color.black;
            marker.style.borderBottomColor = Color.black;
            marker.style.borderLeftColor   = Color.black;
            marker.style.borderRightColor  = Color.black;
            marker.tooltip                 = $"{evt.functionName}\n({evt.time:F2}s)";

            // 點擊事件：可以在這裡做 "移除" 或 "編輯" 功能
            marker.RegisterCallback<MouseDownEvent>(e => {
                Debug.Log($"Select Event: {evt.functionName}");
                e.StopPropagation();
            });

            _clipBar.Add(marker);
        }
    }

    // --- 輸入處理 ---
    void SetupInputEvents() {
        _trackContainer.RegisterCallback<PointerDownEvent>(e => {
            if (_targetScript.targetClip == null) {
                return;
            }

            _trackContainer.CapturePointer(e.pointerId);
            UpdateCursor(e.localPosition.x);
        });

        _trackContainer.RegisterCallback<PointerMoveEvent>(e => {
            if (_targetScript.targetClip == null || !_trackContainer.HasPointerCapture(e.pointerId)) {
                return;
            }

            UpdateCursor(e.localPosition.x);
        });

        _trackContainer.RegisterCallback<PointerUpEvent>(e => {
            if (_trackContainer.HasPointerCapture(e.pointerId)) {
                _trackContainer.ReleasePointer(e.pointerId);
            }
        });

        // 右鍵選單：直接讀取 targetScript.gameObject 上的所有腳本
        _trackContainer.RegisterCallback<ContextClickEvent>(evt => {
            if (_targetScript.targetClip == null) {
                return;
            }

            var width     = _trackContainer.resolvedStyle.width;
            var t         = Mathf.Clamp01(evt.localMousePosition.x / width);
            var clickTime = t * _targetScript.targetClip.length;

            ShowAddEventMenu(clickTime);
        });
    }

    void UpdateCursor(float localX) {
        var width = _trackContainer.resolvedStyle.width;
        var t     = Mathf.Clamp01(localX / width);
        _currentTime = t * _targetScript.targetClip.length;
        UpdatePlayheadVisual();
    }

    void UpdatePlayheadVisual() {
        if (_targetScript.targetClip == null || _playhead == null) {
            return;
        }

        var percent = _currentTime / _targetScript.targetClip.length * 100f;
        _playhead.style.left = Length.Percent(percent);
        _timeLabel.text      = $"Time: {_currentTime:F2}s";
    }

    // --- 右鍵選單與反射 (針對這個 GameObject) ---
    void ShowAddEventMenu(float time) {
        var menu     = new GenericMenu();
        var targetGO = _targetScript.gameObject; // 直接拿到該腳本附著的 GameObject

        var scripts = targetGO.GetComponents<MonoBehaviour>();

        foreach (var script in scripts) {
            if (script == null) {
                continue;
            }

            var methods =
                script.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var method in methods)
                if (IsValidEventMethod(method)) {
                    var name = method.Name;
                    menu.AddItem(new GUIContent($"{script.GetType().Name}/{name}"), false, () => AddEvent(time, name));
                }
        }

        menu.ShowAsContext();
    }

    bool IsValidEventMethod(MethodInfo method) {
        if (method.IsSpecialName) {
            return false;
        }

        var parameters = method.GetParameters();
        if (parameters.Length == 0) {
            return true;
        }

        if (parameters.Length == 1) {
            var t = parameters[0].ParameterType;

            return t == typeof(float) || t == typeof(int) || t == typeof(string) || t == typeof(AnimationEvent);
        }

        return false;
    }

    void AddEvent(float time, string funcName) {
        var clip   = _targetScript.targetClip;
        var events = AnimationUtility.GetAnimationEvents(clip).ToList();
        events.Add(new AnimationEvent { time = time, functionName = funcName });
        AnimationUtility.SetAnimationEvents(clip, events.ToArray());
        RebuildTimeline();
    }
}