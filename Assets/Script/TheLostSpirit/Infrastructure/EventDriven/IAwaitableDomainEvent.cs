using Cysharp.Threading.Tasks;

namespace TheLostSpirit.Infrastructure.EventDriven
{
    public interface IAwaitableDomainEvent : IDomainEvent
    {
        UniTaskCompletionSource Completion { get; }
    }
}