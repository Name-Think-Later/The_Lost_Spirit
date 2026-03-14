using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using TheLostSpirit.Infrastructure;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// ================================================================
// [AnimancerMigrate] ClipTransitionTimelineViewer
// 職責：協調者（Orchestrator）
//   - 持有 TimelineToolbarView + TimelineTrackView
//   - 實作 ITimelineViewerController（供 Manipulator 回呼）
//   - 讀寫 SerializedProperty（事件 CRUD）
//   - 暴露 onMarkerSelected / onTimeScrubbed / onDetached 給 Drawer
// ================================================================

namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    public class ClipTransitionTimelineViewer : VisualElement, ITimelineViewerController
    {
        // ── 子 View ────────────────────────────────────────────
        readonly TimelineToolbarView _toolbar;
        readonly TimelineTrackView _trackView;

        // ── 資料狀態 ───────────────────────────────────────────
        AnimationClip _clip;
        float _clipLength;
        float _currentNormalized;
        SerializedProperty _transitionProp;
        SerializedProperty _eventDataListProp;
        int _selectedIndex = -1;

        // ── Drag Guard ─────────────────────────────────────────
        bool _suppressReload;
        bool _pendingReload;

        // ── Public Events ──────────────────────────────────────
        /// <summary>點擊 Marker 時觸發，參數為 index（-1 = 取消選取）。</summary>
        public event Action<int> onMarkerSelected;

        /// <summary>Playhead 移動時觸發，參數為時間（秒）。用於外部驅動 AnimationMode 預覽。</summary>
        public event Action<float> onTimeScrubbed;

        /// <summary>Viewer 從 Panel 分離時觸發。</summary>
        public event Action onDetached;

        // ================================================================

        public ClipTransitionTimelineViewer()
        {
            style.marginTop = 4;
            style.marginBottom = 4;

            _toolbar = new TimelineToolbarView();
            _toolbar.onAddEvent += () => OnRequestAddEvent(_currentNormalized);
            Add(_toolbar);

            _trackView = new TimelineTrackView(this);
            Add(_trackView);

            RegisterCallback<DetachFromPanelEvent>(_ => onDetached?.Invoke());

            SetNoClip();
        }

        // ================================================================
        // Public API
        // ================================================================

        /// <summary>傳入 ClipTransition 的 SP，讀取並顯示（可編輯）。</summary>
        public void LoadFromSerializedProperty(
            SerializedProperty transitionProperty,
            SerializedProperty eventDataListProperty = null)
        {
            _transitionProp = transitionProperty;
            _eventDataListProp = eventDataListProperty;

            if (transitionProperty == null) { _selectedIndex = -1; SetNoClip(); return; }

            var clipProp = transitionProperty.FindPropertyRelative("_Clip");
            var newClip = clipProp?.objectReferenceValue as AnimationClip;

            if (newClip != _clip) _selectedIndex = -1;   // Clip 改變才重置選取
            _clip = newClip;

            if (_clip == null) { SetNoClip(); return; }

            _clipLength = _clip.length;
            Reload();
        }

        /// <summary>直接傳入 ClipTransition 實例（唯讀，無法寫回 SP）。</summary>
        public void LoadFromTransition(ClipTransition transition)
        {
            _transitionProp = null;
            _eventDataListProp = null;
            _selectedIndex = -1;

            if (transition?.Clip == null) { SetNoClip(); return; }

            _clip = transition.Clip;
            _clipLength = _clip.length;

            var normalEvents = new List<float>();
            float? endEventTime = null;
            var events = transition.Events;

            if (events != null)
            {
                for (int i = 0; i < events.Count; i++)
                    normalEvents.Add(events[i].normalizedTime);

                var endNT = events.NormalizedEndTime;
                if (!float.IsNaN(endNT)) endEventTime = endNT;
            }

            ShowClip(normalEvents, endEventTime);
            _toolbar.SetAddEnabled(false);
        }

        /// <summary>移動 Playhead 到指定 normalizedTime（0~1）。</summary>
        public void SetPlayHeadNormalized(float n)
        {
            _currentNormalized = Mathf.Clamp01(n);
            _trackView.SetPlayHead(_currentNormalized);
            _toolbar.SetTime(_currentNormalized * _clipLength, _clip ? _clip.frameRate : 60f);
            onTimeScrubbed?.Invoke(_currentNormalized * _clipLength);
        }

        /// <summary>程式化選取指定 index 的 Marker。</summary>
        public void SelectMarker(int index)
        {
            _selectedIndex = index;
            _trackView.HighlightMarker(_selectedIndex);
            onMarkerSelected?.Invoke(_selectedIndex);
        }

        /// <summary>
        /// 根據選取的 EventData 展示 DurationFrame 色塊。
        /// 對應原版 UpdateDurationVisuals。
        /// </summary>
        public void UpdateDurationVisuals(EventData eventData, int selectedEventIndex, float eventTimeSec)
            => _trackView.UpdateDurationVisuals(eventData, selectedEventIndex, eventTimeSec);

        /// <summary>隱藏 DurationBar。</summary>
        public void HideDurationBar() => _trackView.HideDurationBar();

#if UNITY_EDITOR
        /// <summary>在 Scene 中繪製選取 CombatStep 的攻擊範圍 Gizmo。</summary>
        public bool DrawDebugRange() => _trackView.DrawDebugRange();
#endif

        /// <summary>設定角色 Target，用於 DrawDebugRange。</summary>
        public void SetCharacterTarget(GameObject target) => _trackView.SetCharacterTarget(target);

        // ================================================================
        // ITimelineViewerController
        // ================================================================

        public void OnScrub(float normalizedRatio)
        {
            SetPlayHeadNormalized(SnapToFrame(normalizedRatio));
        }

        public void OnRequestAddEvent(float normalizedRatio)
        {
            if (_transitionProp == null || _clip == null) return;
            AddEventAt(SnapToFrame(normalizedRatio));
        }

        public void OnMoveEvent(int eventIndex, float normalizedRatio)
        {
            if (_transitionProp == null || _clip == null) return;
            MoveEventAt(eventIndex, normalizedRatio); // already snapped by Manipulator
        }

        public void OnRequestDeleteEvent(int eventIndex)
        {
            if (_transitionProp == null || _clip == null) return;
            DeleteEventAt(eventIndex);
        }

        public void SelectEvent(int index)
        {
            _selectedIndex = index;
            _trackView.HighlightMarker(_selectedIndex);
            onMarkerSelected?.Invoke(_selectedIndex);
        }

        public void OnBeginDrag()
        {
            _suppressReload = true;
            _pendingReload = false;
        }

        public void OnEndDrag()
        {
            _suppressReload = false;
            if (_pendingReload) { _pendingReload = false; Reload(); }
        }

        public AnimationClip GetClip() => _clip;

        // ================================================================
        // Private: Reload
        // ================================================================

        void Reload()
        {
            if (_suppressReload) { _pendingReload = true; return; }
            if (_transitionProp == null || _clip == null) return;

            var eventsProp = _transitionProp.FindPropertyRelative("_Events");
            ReadNormalizedTimes(eventsProp, out var normalEvents, out var endEventTime);
            SyncEventDataListSize(normalEvents.Count);
            ShowClip(normalEvents, endEventTime);
            _toolbar.SetAddEnabled(true);
        }

        void ShowClip(List<float> normalEvents, float? endEventTime)
        {
            _trackView.SetClip(_clip);
            _trackView.ClearMarkers();

            for (int i = 0; i < normalEvents.Count; i++)
                _trackView.AddMarker(i, normalEvents[i], i == _selectedIndex, this);

            if (endEventTime.HasValue)
                _trackView.AddEndEventLine(endEventTime.Value, _clipLength);

            SetPlayHeadNormalized(_currentNormalized);
        }

        void SetNoClip()
        {
            _clip = null;
            _clipLength = 0f;
            _transitionProp = null;

            _trackView.SetClip(null);
            _toolbar.SetTime(0f, 60f);
            _toolbar.SetAddEnabled(false);
        }

        // ================================================================
        // Private: EventData List Sync
        // ================================================================

        void SyncEventDataListSize(int normalEventCount)
        {
            if (_eventDataListProp == null || !_eventDataListProp.isArray) return;

            int current = _eventDataListProp.arraySize;
            if (current == normalEventCount) return;

            Undo.RecordObject(_eventDataListProp.serializedObject.targetObject, "Sync EventData List");

            if (current < normalEventCount)
                for (int i = current; i < normalEventCount; i++)
                    _eventDataListProp.InsertArrayElementAtIndex(i);
            else
                for (int i = current - 1; i >= normalEventCount; i--)
                    _eventDataListProp.DeleteArrayElementAtIndex(i);

            _eventDataListProp.serializedObject.ApplyModifiedProperties();
        }

        // ================================================================
        // Private: Event CRUD
        // ================================================================

        void AddEventAt(float normalizedTime)
        {
            var timesProp = GetNormalizedTimesProp();
            if (timesProp == null) return;

            var times = ReadRawTimes(timesProp);
            if (times.Take(times.Count - 1).Any(t => Mathf.Approximately(t, normalizedTime)))
            {
                Debug.LogWarning($"[ClipTransitionViewer] normalizedTime={normalizedTime:F4} 已存在，跳過。");
                return;
            }

            float endEvt = times.Count > 0 ? times[times.Count - 1] : float.NaN;
            var normal = times.Take(times.Count - 1).ToList();
            normal.Add(normalizedTime);
            normal.Sort();
            int insertedIndex = normal.IndexOf(normalizedTime);
            normal.Add(endEvt);

            WriteRawTimes(timesProp, normal);

            if (_eventDataListProp != null && _eventDataListProp.isArray)
            {
                _eventDataListProp.InsertArrayElementAtIndex(insertedIndex);
                _eventDataListProp.GetArrayElementAtIndex(insertedIndex).managedReferenceValue =
                    new EventData();
                _eventDataListProp.serializedObject.ApplyModifiedProperties();
            }

            _selectedIndex = insertedIndex;
            Reload();
            onMarkerSelected?.Invoke(_selectedIndex);
        }

        void MoveEventAt(int index, float normalizedTime)
        {
            var timesProp = GetNormalizedTimesProp();
            if (timesProp == null) return;

            var times = ReadRawTimes(timesProp);
            int normalCount = times.Count - 1;
            if (index < 0 || index >= normalCount) return;

            for (int i = 0; i < normalCount; i++)
            {
                if (i != index && Mathf.Approximately(times[i], normalizedTime))
                {
                    Debug.LogWarning($"[ClipTransitionViewer] 位置 {normalizedTime:F4} 已有事件，移動取消。");
                    Reload();
                    return;
                }
            }

            float endEvt = times[times.Count - 1];
            var normal = times.Take(normalCount).ToList();
            normal[index] = normalizedTime;

            var sortOrder = Enumerable.Range(0, normalCount).ToList();
            sortOrder.Sort((a, b) => normal[a].CompareTo(normal[b]));

            var sortedNormal = sortOrder.Select(i2 => normal[i2]).ToList();
            sortedNormal.Add(endEvt);

            WriteRawTimes(timesProp, sortedNormal);

            if (_eventDataListProp != null && _eventDataListProp.isArray &&
                _eventDataListProp.arraySize == normalCount)
                ReorderEventDataList(sortOrder);

            _selectedIndex = sortOrder.IndexOf(index);
            Reload();
            onMarkerSelected?.Invoke(_selectedIndex);
        }

        void DeleteEventAt(int index)
        {
            var timesProp = GetNormalizedTimesProp();
            if (timesProp == null) return;

            var times = ReadRawTimes(timesProp);
            int normalCount = times.Count - 1;
            if (index < 0 || index >= normalCount) return;

            float endEvt = times[times.Count - 1];
            var normal = times.Take(normalCount).ToList();
            normal.RemoveAt(index);
            normal.Add(endEvt);

            WriteRawTimes(timesProp, normal);

            if (_eventDataListProp != null && _eventDataListProp.isArray &&
                index < _eventDataListProp.arraySize)
            {
                Undo.RecordObject(_eventDataListProp.serializedObject.targetObject, "Delete EventData");
                _eventDataListProp.DeleteArrayElementAtIndex(index);
                _eventDataListProp.serializedObject.ApplyModifiedProperties();
            }

            _selectedIndex = -1;
            Reload();
            onMarkerSelected?.Invoke(-1);
        }

        // ================================================================
        // Private: SP Helpers
        // ================================================================

        void ReorderEventDataList(List<int> sortOrder)
        {
            var snapshot = new List<EventData>();
            for (int i = 0; i < sortOrder.Count; i++)
                snapshot.Add(GetEventData(i));

            Undo.RecordObject(_eventDataListProp.serializedObject.targetObject, "Reorder EventData");

            for (int i = 0; i < sortOrder.Count; i++)
                _eventDataListProp.GetArrayElementAtIndex(i).managedReferenceValue = snapshot[sortOrder[i]];

            _eventDataListProp.serializedObject.ApplyModifiedProperties();
        }

        EventData GetEventData(int index)
        {
            if (_eventDataListProp == null || !_eventDataListProp.isArray ||
                index < 0 || index >= _eventDataListProp.arraySize) return null;

            return _eventDataListProp.GetArrayElementAtIndex(index).managedReferenceValue as EventData;
        }

        SerializedProperty GetNormalizedTimesProp()
        {
            if (_transitionProp == null) return null;
            var eventsProp = _transitionProp.FindPropertyRelative("_Events");
            return eventsProp?.FindPropertyRelative("_NormalizedTimes");
        }

        static List<float> ReadRawTimes(SerializedProperty timesProp)
        {
            var list = new List<float>();
            if (timesProp == null || !timesProp.isArray) return list;
            for (int i = 0; i < timesProp.arraySize; i++)
                list.Add(timesProp.GetArrayElementAtIndex(i).floatValue);
            return list;
        }

        static void WriteRawTimes(SerializedProperty timesProp, List<float> times)
        {
            Undo.RecordObject(timesProp.serializedObject.targetObject, "Modify Animancer Events");
            timesProp.arraySize = times.Count;
            for (int i = 0; i < times.Count; i++)
                timesProp.GetArrayElementAtIndex(i).floatValue = times[i];
            timesProp.serializedObject.ApplyModifiedProperties();
        }

        static void ReadNormalizedTimes(
            SerializedProperty eventsProp,
            out List<float> normalEvents,
            out float? endEventTime)
        {
            normalEvents = new List<float>();
            endEventTime = null;

            if (eventsProp == null) return;

            var timesProp = eventsProp.FindPropertyRelative("_NormalizedTimes");
            if (timesProp == null || !timesProp.isArray || timesProp.arraySize == 0) return;

            int lastIndex = timesProp.arraySize - 1;
            for (int i = 0; i < timesProp.arraySize; i++)
            {
                float v = timesProp.GetArrayElementAtIndex(i).floatValue;
                if (i == lastIndex) { if (!float.IsNaN(v)) endEventTime = v; }
                else normalEvents.Add(v);
            }
        }

        float SnapToFrame(float normalizedRatio)
        {
            if (_clip == null || _clipLength <= 0f) return normalizedRatio;
            var timeSec = normalizedRatio * _clipLength;
            var snapped = Mathf.RoundToInt(timeSec * _clip.frameRate) / _clip.frameRate;
            return Mathf.Clamp01(snapped / _clipLength);
        }
    }
}