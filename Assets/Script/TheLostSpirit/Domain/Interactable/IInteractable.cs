using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Interactable {
    public interface IInteractable :
        IEntity<IInteractableID>, ITransformProvider {
        public bool CanInteract { get; }
        public void InFocus();
        public void OutOfFocus();
        public void Interacted();
    }
}