using System;
using Script.TheLostSpirit.EventBusModule;
using Script.TheLostSpirit.MapSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.TheLostSpirit.EntryPoint.MainScene {

    public class MainSceneEntryPoint : MonoBehaviour {
        EventBus _eventBus = new EventBus();

        [SerializeField]
        MapGenerationSetting _mapGenerationSetting;


        MapController _mapController;
        
        
        void Awake() {
            _mapController = new MapController(_mapGenerationSetting);

            _mapController.Generate();
        }


        [Button(ButtonSizes.Medium), DisableInEditorMode]
        public void GenerateMap() {
            _mapController.Clear();
            _mapController.Generate();
        }
    }
}