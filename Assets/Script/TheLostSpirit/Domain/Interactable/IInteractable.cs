using TheLostSpirit.Domain.Portal.Event;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.Domain;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Interactable {
    public interface IInteractable :
        IEntity<IInteractableID>, IPositionProvider {
        public bool CanInteract { get; }
        public void InFocus();
        public void OutOfFocus();
        public void Interacted();
    }
}