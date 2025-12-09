using Sirenix.OdinInspector;
using TheLostSpirit.Application.UseCase;
using TheLostSpirit.Application.UseCase.Map;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Context.Room;
using TheLostSpirit.Domain;
using TheLostSpirit.Infrastructure;
using UnityEngine;

namespace TheLostSpirit.Context.EntryPoint.MainScene
{
    public class MainSceneContext : MonoBehaviour
    {
        [SerializeField]
        PlayerContext _playerContext;

        [SerializeField, SceneObjectsOnly]
        PlayerInstanceContext _playerInstanceContext;

        [SerializeField]
        UserInputContext _userInputContext;

        [SerializeField, SceneObjectsOnly]
        RoomContext _roomContext;

        [SerializeField, SceneObjectsOnly]
        PortalContext _portalContext;

        ClearMapUseCase _clearMapUseCase;

        GenerateMapUseCase _generateMapUseCase;

        void Awake() {
            Construct();
        }

        void Start() {
            TheLostSpirits();
        }

        MainSceneContext Construct() {
            AppScope.Initialize(new EventBus());

            _userInputContext.Construct();
            _playerContext.Construct();
            _portalContext.Construct(_playerContext);
            _roomContext.Construct(_portalContext);


            _generateMapUseCase =
                new GenerateMapUseCase(
                    _roomContext.RoomRepository,
                    _roomContext.CreateRoomUseCase,
                    _roomContext.ConnectRoomUseCase
                );

            _clearMapUseCase = new ClearMapUseCase(
                _roomContext.RoomRepository,
                _roomContext.RoomViewModelStore,
                _roomContext.RoomInstanceFactory
            );

            return this;
        }

        void TheLostSpirits() {
            _playerInstanceContext.Construct(_playerContext, _userInputContext);
            _generateMapUseCase.Execute(new GenerateMapUseCase.Input(3));
        }

        [DisableInEditorMode, Button(ButtonSizes.Medium)]
        public void GenerateMap() {
            _clearMapUseCase.Execute();
            _generateMapUseCase.Execute(new GenerateMapUseCase.Input(3));
        }
    }
}