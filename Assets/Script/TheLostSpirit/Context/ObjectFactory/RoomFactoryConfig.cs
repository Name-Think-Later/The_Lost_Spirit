using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Context.Room;
using UnityEngine;

namespace TheLostSpirit.Context.ObjectFactory {
    [Serializable, HideLabel, Indent(2)]
    [FoldoutGroup("Factory Config")]
    public class RoomFactoryConfig {
        [SerializeField]
        [AssetList(Path = "Prefabs/RoomPattern/Sample", AutoPopulate = true)]
        RoomObjectContext[] _roomPattern;

        [Space]
        [SerializeField, SceneObjectsOnly]
        Grid _roomDrawingGrid;

        [SerializeField]
        float _offset;

        public Grid RoomDrawingGrid => _roomDrawingGrid;

        public RoomObjectContext[] RoomPattern => _roomPattern;

        public float Offset => _offset;
    }
}