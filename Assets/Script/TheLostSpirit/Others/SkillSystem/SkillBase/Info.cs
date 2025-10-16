using System;
using UnityEngine;

namespace TheLostSpirit.Others.SkillSystem.SkillBase {
    [Serializable]
    public class Info {
        [SerializeField]
        string _name;

        [SerializeField, Multiline]
        string _description;

        public string Name {
            get => _name;
            set => _name = value;
        }

        public string Description {
            get => _description;
            set => _description = value;
        }
    }
}