using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheLostSpirit.Context.Room
{
    [Serializable]
    public class RoomObjectFactoryConfig
    {
        [SerializeField, AssetList(AutoPopulate = true)]
        public RoomObjectContext[] roomObjectContexts;

        [SerializeField, SceneObjectsOnly]
        public Grid roomDrawingGrid;

        [SerializeField]
        public float horizontalOffset;
    }
}