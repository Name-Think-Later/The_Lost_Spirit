using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Context.Room
{
    [Serializable]
    public class RoomInstanceFactoryConfig
    {
        [SerializeField, AssetList(AutoPopulate = true)]
        public RoomInstanceContext[] roomObjectContexts;

        [SerializeField, SceneObjectsOnly]
        public Grid roomDrawingGrid;

        [SerializeField]
        public float horizontalOffset;
    }
}