using Sirenix.OdinInspector;
using TheLostSpirit.Application.UseCase.Contract;
using TheLostSpirit.Application.UseCase.Map;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Context.Room;
using TheLostSpirit.Presentation.View.Input;
using UnityEngine;

namespace TheLostSpirit.Context.MainScene
{
    public class MainSceneContext : MonoBehaviour
    {
        [SerializeField, SceneObjectsOnly]
        RoomContext _roomContext;

        [SerializeField, SceneObjectsOnly]
        PortalContext _portalContext;

        [SerializeField, SceneObjectsOnly]
        PlayerObjectContext _playerObjectContext;


        ActionMap          _actionMap;
        GenerateMapUseCase _generateMapUseCase;
        ClearMapUseCase    _clearMapUseCase;

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

            _generateMapUseCase =
                new GenerateMapUseCase(
                    _roomContext.RoomRepository,
                    _roomContext.CreateRoomUseCase,
                    _roomContext.ConnectRoomUseCase
                );

            _clearMapUseCase = new ClearMapUseCase(
                _roomContext.RoomRepository,
                _roomContext.RoomViewModelStore,
                _roomContext.RoomObjectFactory
            );
        }

        void TheLostSpirits() {
            _playerObjectContext.Instantiate();

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