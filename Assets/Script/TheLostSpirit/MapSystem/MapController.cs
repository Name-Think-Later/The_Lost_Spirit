using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Script.TheLostSpirit.MapSystem {
    public class MapController {
        readonly MapGenerationSetting _setting;
        readonly List<RoomController> _roomControllers;

        public MapController(MapGenerationSetting setting) {
            _setting         = setting;
            _roomControllers = new List<RoomController>();
        }

        public void Generate() {
            RoomCreation();

            RoomConnection();

            RoomArrangement();
        }

        void RoomCreation() {
            for (int i = 0; i < _setting.GenerateAmount; i++) {
                var index          = Random.Range(0, _setting.RoomPattern.Length);
                var pattern        = _setting.RoomPattern[index];
                var roomReference  = InstantiateRoomReference(pattern);
                var roomController = new RoomController(roomReference);
                _roomControllers.Add(roomController);
            }
        }

        void RoomArrangement() {
            var horizontalOffset = 0f;
            _roomControllers.ForEach(roomController => {
                var vectorOffset = Vector2.right * horizontalOffset;
                roomController.SetPosition(vectorOffset);
                horizontalOffset += _setting.Offset;
            });
        }

        void RoomConnection() {
            _roomControllers.ForEach(roomLeft => {
                var tryGetPortalLeft = roomLeft.InactivePortals.Take(1);

                tryGetPortalLeft.ForEach(portalLeft => {
                    var roomFilter =
                        _roomControllers.Where(roomRight => {
                            var notSameRoom = roomRight != roomLeft;

                            var notConnectToEachOther =
                                roomRight
                                    .ActivePortals
                                    .Select(portal => portal.Destination.Room)
                                    .All(room => room != roomLeft);

                            return notSameRoom && notConnectToEachOther;
                        });

                    var tryGetRandomRoom = roomFilter.Shuffle().Take(1);

                    tryGetRandomRoom.ForEach(roomRight => {
                        var tryGetPortalRight = roomRight.InactivePortals.Take(1);

                        tryGetPortalRight.ForEach(portalLeft.Connect);
                    });
                });
            });
        }

        RoomReference InstantiateRoomReference(RoomReference pattern) {
            var original = pattern.gameObject;
            var parent   = _setting.RoomDrawingGrid.transform;
            var instance = Object.Instantiate(original, parent);

            return instance.GetComponent<RoomReference>();
        }

        public void Clear() {
            _roomControllers.ForEach(room => room.Destroy());
            _roomControllers.Clear();
        }
    }
}