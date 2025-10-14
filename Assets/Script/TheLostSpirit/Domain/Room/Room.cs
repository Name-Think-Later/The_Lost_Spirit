using System.Collections.Generic;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Room {
    public class Room {
        public List<RoomID> AssociatedRoom { get; } = new();
        public List<PortalID> IncludedPortal { get; } = new();
    }
}