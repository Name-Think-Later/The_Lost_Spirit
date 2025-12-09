using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Identity.ConfigID
{
    [Serializable]
    public record SkillConfigID : IConfigID
    {
        [SerializeField, LabelText("Id")]
        string _value;

        public SkillConfigID(string value) {
            _value = value;
        }

        public string Value => _value;
    }
}