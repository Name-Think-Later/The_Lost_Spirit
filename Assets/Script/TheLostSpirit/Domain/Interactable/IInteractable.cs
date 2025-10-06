using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.DomainDriven;

namespace TheLostSpirit.Domain.Interactable {
    public interface IInteractable :
        IEntity<IInteractableID>, IPositionProvider {
        public void InFocus();
        public void OutOfFocus();
        public void Interacted();
    }
}