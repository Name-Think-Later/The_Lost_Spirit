using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Domain.Skill.Manifest.Event
{
    public record AsyncManifestActivatedEvent(FormulaPayload Payload) : IAsyncDomainEvent
    {
        readonly UniTaskCompletionSource _completion = new();

        public FormulaPayload Payload { get; } = Payload;
        public void Complete() => _completion.TrySetResult();

        public UniTask Await() => _completion.Task;
    }
}