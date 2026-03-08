using System;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    interface IAsyncDomainEventHandler<in TEvent> : IDisposable where TEvent : IAsyncDomainEvent
    {
        protected UniTask Handle(TEvent domainEvent);
    }
}