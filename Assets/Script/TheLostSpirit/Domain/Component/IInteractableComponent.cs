using TheLostSpirit.Domain.Port.ReadOnly;

namespace TheLostSpirit.Domain.Component
{
    public interface IInteractableComponent
    {
        IReadOnlyTransform ReadOnlyTransform { get; }
        public void Interacted();
        public void Detected();
        public void Undetected();
    }
}