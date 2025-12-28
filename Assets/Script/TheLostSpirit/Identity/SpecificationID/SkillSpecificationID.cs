using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Identity.SpecificationID
{
    [Serializable, HideLabel]
    public record SkillSpecificationID : ISpecificationID
    {
        [SerializeField, LabelText("Id"), Required]
        string _value;

        public SkillSpecificationID(string value) {
            _value = value;
        }

        public string Value => _value;
    }
}