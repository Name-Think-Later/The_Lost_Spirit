using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.TheLostSpirit.MapSystem {
    [Serializable]
    public class MapGenerationSetting {
        [SerializeField]
        Grid _roomDrawingGrid;

        [SerializeField]
        int _generateAmount;

        [SerializeField]
        float _offset;

        [SerializeField, AssetList]
        RoomReference[] _roomPattern;

        public Grid RoomDrawingGrid => _roomDrawingGrid;
        public int GenerateAmount => _generateAmount;

        public float Offset => _offset;

        public RoomReference[] RoomPattern => _roomPattern;
    }
}