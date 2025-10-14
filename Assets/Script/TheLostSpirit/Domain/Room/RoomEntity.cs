using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.Domain;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Room {
    public class RoomEntity : IEntity<RoomID> {
        readonly Room      _room;
        readonly IRoomMono _roomMono;

        public RoomID ID { get; }

        public IReadOnlyList<PortalID> IncludedPortal => _room.IncludedPortal;

        public RoomEntity(RoomID id, IRoomMono mono) {
            ID = id;

            _room = new Room();

            _roomMono = mono;
            _roomMono.Initialize(ID);
        }


        public void IncludePortal(PortalID portal) {
            _room.IncludedPortal.Add(portal);
        }

        public void Associate(RoomID other) {
            _room.AssociatedRoom.Add(other);
        }

        public bool IsAssociate(RoomID other) {
            return _room.AssociatedRoom.Contains(other);
        }
    }
}