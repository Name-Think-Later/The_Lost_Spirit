using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace TheLostSpirit.MapSystem {
    public class MapController {
        readonly RoomFactoryConfig _setting;
        readonly List<RoomController> _roomControllers;

        public MapController(RoomFactoryConfig setting) {
            _setting         = setting;
            _roomControllers = new List<RoomController>();
        }

        public void Generate() {
            RoomCreation();

            RoomConnection();

            RoomArrangement();
        }

        void RoomCreation() {
            // for (int i = 0; i < _setting.GenerateAmount; i++) {
            //     var index          = Random.Range(0, _setting.RoomPattern.Length);
            //     var pattern        = _setting.RoomPattern[index];
            //     //var roomReference  = InstantiateRoomReference(pattern);
            //     //var roomController = new RoomController(roomReference);
            //     //_roomControllers.Add(roomController);
            // }
        }

        void RoomConnection() {
            
            MainConnection();
            
            BranchConnection();
        }

        void MainConnection() {
            _roomControllers
                .Window(2)
                .ForEach(pair => {
                    var current = pair[0];
                    var next    = pair[1];

                    var currentPortal = current.InactivePortals.Shuffle().First();
                    var nextPortal    = next.InactivePortals.Shuffle().First();
                    //currentPortal.Connect(nextPortal);
                });
        }

        void BranchConnection() {
            /*
            _roomControllers.ForEach(roomLeft => {
                var inactivePortalCount = roomLeft.InactivePortals.Count();
                var cdfSampler          = new CdfSampler(inactivePortalCount + 1);
                var branchAmount        = cdfSampler.Next();

                
                var tryGetPortalLefts = roomLeft.InactivePortals.Take(branchAmount);

                tryGetPortalLefts.ForEach(portalLeft => {
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
            */
        }


        void RoomArrangement() {
            var horizontalOffset = 0f;
            _roomControllers.ForEach(roomController => {
                var vectorOffset = Vector2.right * horizontalOffset;
                roomController.SetPosition(vectorOffset);
                //horizontalOffset += _setting.Offset;
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