using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.ConfigID;

namespace TheLostSpirit.Domain.Skill.Manifest.Event
{
    public record AsyncManifestActivatedEvent(
        ManifestationConfigID ManifestationConfigID,
        FormulaPayload        Payload
    ) : IAsyncDomainEvent
    {
        readonly UniTaskCompletionSource _completion = new UniTaskCompletionSource();

        public FormulaPayload Payload { get; } = Payload;
        public ManifestationConfigID ManifestationConfigID { get; } = ManifestationConfigID;

        public void Complete() {
            _completion.TrySetResult();
        }

        public UniTask Await() {
            return _completion.Task;
        }
    }
}