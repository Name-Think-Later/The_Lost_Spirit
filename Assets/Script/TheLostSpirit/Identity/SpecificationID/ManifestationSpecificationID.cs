using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Identity.SpecificationID
{
    [Serializable, HideLabel]
    public record ManifestationSpecificationID : ISpecificationID
    {
        [SerializeField, LabelText("Id"), Required]
        string _value;

        public ManifestationSpecificationID(string value) {
            _value = value;
        }

        public string Value => _value;
    }
}