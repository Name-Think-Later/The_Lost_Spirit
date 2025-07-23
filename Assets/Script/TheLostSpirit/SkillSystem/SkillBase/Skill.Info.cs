using System;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.SkillBase {
    public partial class Skill {
        [Serializable]
        public class Info {
            [SerializeField]
            string _name;

            [SerializeField, Multiline]
            string _description;


            public Info(string name) {
                _name = name;
            }

            public string Name => _name;
            public string Description => _description;
        }
    }
}