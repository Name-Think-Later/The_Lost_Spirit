using Sirenix.OdinInspector;
using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Application.UseCase.Map;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Playground;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Context.Room;
using TheLostSpirit.Presentation.View.Input;
using TheLostSpirit.Presentation.ViewModel;
using UnityEngine;

namespace TheLostSpirit.Context.MainScene
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


        ActionMap        _actionMap;
        GeneralInputView _generalInputView;

        GenerateMapUseCase _generateMapUseCase;
        ClearMapUseCase    _clearMapUseCase;

        void Awake() {
            Construct();
        }

        void Start() {
            TheLostSpirits();
        }

        MainSceneContext Construct() {
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
            _generateMapUseCase.Execute(new(3));
        }

        [DisableInEditorMode]
        [Button(ButtonSizes.Medium)]
        public void GenerateMap() {
            _clearMapUseCase.Execute();
            _generateMapUseCase.Execute(new(3));
        }
    }
}