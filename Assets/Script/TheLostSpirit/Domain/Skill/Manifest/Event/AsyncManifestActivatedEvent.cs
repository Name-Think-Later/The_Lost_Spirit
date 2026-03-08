using System;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Domain.Skill.Manifest.Event
{
    public record AsyncManifestActivatedEvent(
        ManifestationSpecificationID ManifestationSpecificationID,
        Guid                         SequentID
    ) : IAsyncDomainEvent
    {
        readonly UniTaskCompletionSource _completion = new UniTaskCompletionSource();

        public ManifestationSpecificationID ManifestationSpecificationID { get; } = ManifestationSpecificationID;
        public Guid SequentID { get; } = SequentID;

        public void Complete() {
            _completion.TrySetResult();
        }

        public UniTask Await() {
            return _completion.Task;
        }
    }
}