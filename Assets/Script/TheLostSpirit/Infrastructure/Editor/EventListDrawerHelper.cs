#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

// 引用 Action

namespace TheLostSpirit.Infrastructure.Editor
{
    public static class EventListDrawerHelper
    {
        static readonly Color _selectionColor = new Color(0.4f, 0.5f, 0.90f, 0.1f);
        static          Rect  _currentRect;

        public static void BeginElementGUI() {
            _currentRect = EditorGUILayout.BeginVertical(GUIStyle.none);
        }
        
        public static void EndElementGUI(int index, ref int selectedIndex) {
            EditorGUILayout.EndVertical();

            if (_currentRect.width < 1) {
                _currentRect = GUILayoutUtility.GetLastRect();
            }

            var evt        = Event.current;
            var canRepaint = evt.type == EventType.Repaint;
            var isSelected = index == selectedIndex;

            // 1. 繪製反白 (Repaint)

            if (isSelected && canRepaint) {
                var bgRect = _currentRect;
                EditorGUI.DrawRect(bgRect, _selectionColor);
            }

            // 2. 偵測點擊
            var isClick    = evt.type is EventType.MouseUp or EventType.Used && evt.button == 0;
            var isContains = _currentRect.Contains(evt.mousePosition);

            if (isClick && isContains) {
                selectedIndex = index; // 觸發選取
            }
        }
    }
}


#endif