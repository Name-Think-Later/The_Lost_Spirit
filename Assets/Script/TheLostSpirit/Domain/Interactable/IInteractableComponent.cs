using TheLostSpirit.Domain.Port.ReadOnly;

namespace TheLostSpirit.Domain.Interactable
{
    public interface IInteractableComponent
    {
        IReadOnlyTransform ReadOnlyTransform { get; }
        public void Interacted();
        public void Detected();
        public void Undetected();
    }
}