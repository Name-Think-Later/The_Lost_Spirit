using TheLostSpirit.Identify;

namespace TheLostSpirit.Application.ObjectFactoryContract {
    public interface IPortalFactory {
        PortalID CreateAndRegister();
    }
}