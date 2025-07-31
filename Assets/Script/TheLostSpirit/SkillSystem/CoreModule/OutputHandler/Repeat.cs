using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.CoreModule.OutputHandler {
    [Serializable]
    public class Repeat : Core.BehaviourData.IOutputHandler {
        [SerializeField]
        int _repeatTimes;

        [SerializeField]
        float _interval;

        public int RepeatTimes {
            get => _repeatTimes;
            set => _repeatTimes = value;
        }

        public float Interval {
            get => _interval;
            set => _interval = value;
        }

        public Action OutputAction { get; set; }

        public void HandleOutput() {
            RepeatOutputAsync().Forget();
        }

        async UniTaskVoid RepeatOutputAsync() {
            for (int i = 0; i < _repeatTimes - 1; i++) {
                OutputAction();
                await UniTask.Delay(TimeSpan.FromSeconds(_interval));
            }

            OutputAction();
        }
    }
}