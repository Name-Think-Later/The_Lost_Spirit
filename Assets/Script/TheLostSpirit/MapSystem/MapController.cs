using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using ZLinq.Linq;

namespace Script.TheLostSpirit.MapSystem {
    public class MapController {
        readonly MapGenerationSetting _setting;
        readonly List<RoomController> _roomControllers;

        public MapController(MapGenerationSetting setting) {
            _setting         = setting;
            _roomControllers = new List<RoomController>();
        }

        public void Generate() {
            var horizontalOffset = 0f;
            for (int i = 0; i < _setting.GenerateAmount; i++) {
                var index          = Random.Range(0, _setting.RoomPattern.Length);
                var pattern        = _setting.RoomPattern[index];
                var roomReference  = InstantiateRoomReference(pattern);
                var roomController = new RoomController(roomReference);
                _roomControllers.Add(roomController);

                var vectorOffset = Vector2.right * horizontalOffset;
                roomController.SetPosition(vectorOffset);
                horizontalOffset += _setting.Offset;
            }

            _roomControllers
                .Where(roomLeft => roomLeft.InactivePortals.Any())
                .ForEach(roomLeft => {
                    var portalLeft = roomLeft.InactivePortals.First();

                    _roomControllers
                        .Where(roomRight => roomRight != roomLeft)
                        .Where(roomRight => roomRight.InactivePortals.Any())
                        .Where(roomRight => {
                            return roomRight
                                   .ActivePortals
                                   .Select(portal => portal.Destination.Room)
                                   .All(room => room != roomLeft);
                        })
                        .Shuffle()
                        .Take(1)
                        .ForEach(roomRight => {
                            var portalRight = roomRight.InactivePortals.First();
                            portalLeft.Connect(portalRight);
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