using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Domain.Skill.Core.Event
{
    public record AsyncCoreActivatedEvent(FormulaPayload Payload) : IAsyncDomainEvent
    {
        readonly UniTaskCompletionSource _completion = new UniTaskCompletionSource();
        public FormulaPayload Payload { get; } = Payload;

        public void Complete() {
            _completion.TrySetResult();
        }

        public UniTask Await() {
            return _completion.Task;
        }
    }
}