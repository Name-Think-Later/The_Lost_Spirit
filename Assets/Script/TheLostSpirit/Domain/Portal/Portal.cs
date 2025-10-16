using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Portal {
    public class Portal {
        public PortalID AssociatedPortal { get; set; }
        public bool IsEnable { get; set; } = true;

        public bool HasAssociated => AssociatedPortal != null;
    }
}