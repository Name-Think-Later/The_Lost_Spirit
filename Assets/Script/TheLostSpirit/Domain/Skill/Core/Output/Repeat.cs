using System;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Core.Output
{
    [Serializable]
    public class Repeat : IOutputPolicy
    {
        [SerializeField, Indent]
        public int repeatTimes;

        [SerializeField, Indent]
        public float interval;


        public void HandleOutput(Action output) {
            Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(interval))
                .Take(repeatTimes)
                .Subscribe(_ => output());
        }
    }
}