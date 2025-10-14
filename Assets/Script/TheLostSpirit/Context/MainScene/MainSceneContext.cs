using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.UseCase.Room;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Context.Room;
using TheLostSpirit.Infrastructure.UseCase;
using TheLostSpirit.View.Input;
using UnityEngine;

namespace TheLostSpirit.Context.MainScene {
    public class MainSceneContext : MonoBehaviour {
        [SerializeField, SceneObjectsOnly]
        RoomContext _roomContext;

        [SerializeField, SceneObjectsOnly]
        PortalContext _portalContext;

        [SerializeField, SceneObjectsOnly]
        PlayerObjectContext _playerObjectContext;


        ActionMap _actionMap;


        void Awake() {
            Construct();
        }

        void Start() {
            TheLostSpirits();
        }

        void Construct() {
            _actionMap = new ActionMap();
            _actionMap.Enable();

            var generalInputView = new GeneralInputView(_actionMap.General);

            _playerObjectContext.Construct(generalInputView);
            _portalContext.Construct(_playerObjectContext);
            _roomContext.Construct(_portalContext);
        }

        void TheLostSpirits() {
            _playerObjectContext.Produce();
            var r1 = _roomContext.RoomFactory.CreateRandom();
            var r2 = _roomContext.RoomFactory.CreateRandom();
            var r3 = _roomContext.RoomFactory.CreateRandom();

            _roomContext.ConnectRoomUseCase.Execute(new(r1, r2));
            _roomContext.ConnectRoomUseCase.Execute(new(r2, r3));
        }
    }
}