using System;
using Animancer;
using UnityEngine;

namespace TheLostSpirit.Infrastructure
{
    /// <summary>
    /// 包含 ClipTransition 的可序列化包裝類別。
    /// 在 Inspector 中會顯示 ClipTransition 欄位 + 唯讀事件時間線。
    /// </summary>
    [Serializable]
    public class AnimancerClipEvent
    {
        [SerializeField]
        public ClipTransition transition;
    }
}
