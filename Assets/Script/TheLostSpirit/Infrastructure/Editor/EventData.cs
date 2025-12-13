using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using TheLostSpirit.Infrastructure.Editor; // 請確認有引用您的 Helper namespace
#endif

namespace TheLostSpirit.Infrastructure
{
    [Serializable]
    public class EventData
    {
        [ListDrawerSettings(
            ShowIndexLabels = true,
            OnBeginListElementGUI = nameof(OnBeginListElement),
            OnEndListElementGUI = nameof(OnEndListElement),
            DraggableItems = false
        )]
        public List<CombatAction> Actions = new List<CombatAction>();

        // 這是存檔用的索引 (外部 Timeline 認這個)

        public int SelectedIndex = -1;

#if UNITY_EDITOR

        private void OnBeginListElement(int index) {
            EventListDrawerHelper.BeginElementGUI();
        }

        private void OnEndListElement(int index) {
            EventListDrawerHelper.EndElementGUI(
                index,
                ref SelectedIndex
            );
        }
#endif
    }

    [Serializable]
    public class CombatAction
    {
        [LabelWidth(60)]
        [SuffixLabel("frames", Overlay = true)]
        [MinValue(1)]
        public int DurationFrames = 30;

        [LabelWidth(60)]
        public string Name = "Effect";

        [FoldoutGroup("Details")]
        public int Damage = 10;

        [FoldoutGroup("Details")]
        public GameObject VFX;
    }
}