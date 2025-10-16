using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Others.SkillSystem.CoreModule.OutputHandler {
    [Serializable]
    public class Repeat : IOutputHandler {
        [BoxGroup, SerializeField]
        int _repeatTimes;

        [BoxGroup, SerializeField]
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