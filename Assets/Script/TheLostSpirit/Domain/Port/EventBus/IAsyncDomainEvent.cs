using Cysharp.Threading.Tasks;

namespace TheLostSpirit.Domain.Port.EventBus
{
    public interface IAsyncDomainEvent : IDomainEvent
    {
        public void Complete();
        public UniTask Await();
    }
}