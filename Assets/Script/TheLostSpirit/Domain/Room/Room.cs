using System.Collections.Generic;
using System.Linq;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Room {
    public class Room {
        public List<RoomID> AssociatedRoom { get; } = new();
        public IReadOnlyList<PortalID> IncludedPortal => AvailablePortal.Concat(UnavailablePortal).ToList();
        public List<PortalID> AvailablePortal { get; } = new();
        public List<PortalID> UnavailablePortal { get; } = new();
    }
}