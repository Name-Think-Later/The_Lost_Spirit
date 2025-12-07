using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Portal
{
    public class Portal
    {
        public PortalID AssociatedPortal { get; set; }
        public bool IsEnable { get; set; } = true;
    }
}