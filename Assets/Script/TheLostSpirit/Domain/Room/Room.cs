using System.Collections.Generic;
using System.Linq;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Room
{
    public class Room
    {
        public List<RoomID> AssociatedRoom { get; }
        public List<PortalID> AvailablePortals { get; }
        public List<PortalID> UnavailablePortals { get; }
        public IEnumerable<PortalID> Portals => AvailablePortals.Concat(UnavailablePortals);

        public Room() {
            AssociatedRoom     = new List<RoomID>();
            AvailablePortals   = new List<PortalID>();
            UnavailablePortals = new List<PortalID>();
        }
    }
}