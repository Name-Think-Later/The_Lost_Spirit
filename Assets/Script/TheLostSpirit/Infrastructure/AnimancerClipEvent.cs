using System;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace TheLostSpirit.Infrastructure
{
    /// <summary>
    /// 包含 ClipTransition 的可序列化包裝類別。
    /// <br/>在 Inspector 中顯示 ClipTransition 欄位 + 可互動事件時間線 + EventData Inspector 面板。
    /// <br/><b>eventDataList[i]</b> 對應 ClipTransition._Events._NormalizedTimes[i]（不含最後的 End Event）。
    /// </summary>
    [Serializable]
    public class AnimancerClipEvent
    {
        [SerializeField]
        public ClipTransition transition;

        /// <summary>
        /// 每個 Animancer 普通事件（不含 End Event）對應的 EventData。
        /// index 嚴格與 _Events._NormalizedTimes（排序後，不含最後一格）對應。
        /// </summary>
        [SerializeReference]
        public List<EventData> eventDataList = new List<EventData>();
    }
}
