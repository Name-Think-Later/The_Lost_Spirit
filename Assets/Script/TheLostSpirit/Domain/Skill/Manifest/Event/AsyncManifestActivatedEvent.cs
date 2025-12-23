using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Domain.Skill.Manifest.Event
{
    public record AsyncManifestActivatedEvent(
        ManifestationSpecificationID ManifestationSpecificationID,
        FormulaPayload        Payload
    ) : IAsyncDomainEvent
    {
        readonly UniTaskCompletionSource _completion = new UniTaskCompletionSource();

        public FormulaPayload Payload { get; } = Payload;
        public ManifestationSpecificationID ManifestationSpecificationID { get; } = ManifestationSpecificationID;

        public void Complete() {
            _completion.TrySetResult();
        }

        public UniTask Await() {
            return _completion.Task;
        }
    }
}