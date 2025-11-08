using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Room
{
    public class RoomEntity : IEntity<RoomID>, IDisposable
    {
        readonly Room      _room;
        readonly IRoomMono _roomMono;

        public RoomID ID { get; }

        public IEnumerable<PortalID> Portals => _room.Portals;
        public IEnumerable<PortalID> AvailablePortals => _room.AvailablePortals;
        public IEnumerable<PortalID> UnavailablePortals => _room.UnavailablePortals;

        public RoomEntity(RoomID id, IRoomMono mono) {
            ID = id;

            _room = new Room();

            _roomMono = mono;
            _roomMono.Initialize(ID);
        }


        public void IncludePortal(PortalID portal) {
            _room.AvailablePortals.Add(portal);
        }

        public void TurnPortalToUnavailable(PortalID portal) {
            Contract.Assert(_room.AvailablePortals.Contains(portal), "Portal is not available or not included");
            _room.AvailablePortals.Remove(portal);
            _room.UnavailablePortals.Add(portal);
        }

        public void TurnPortalToAvailable(PortalID portal) {
            Contract.Assert(_room.UnavailablePortals.Contains(portal), "Portal is not unavailable or not included");
            _room.UnavailablePortals.Remove(portal);
            _room.AvailablePortals.Add(portal);
        }

        public void Associate(RoomID other) {
            _room.AssociatedRoom.Add(other);
        }

        public bool IsAssociate(RoomID other) {
            return _room.AssociatedRoom.Contains(other);
        }

        public void Dispose() {
            _roomMono.Destroy();
        }
    }
}