using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Identity.ConfigID
{
    [Serializable]
    public record ManifestationConfigID : IConfigID
    {
        [SerializeField, LabelText("Id")]
        string _value;

        public string Value => _value;
    }
}