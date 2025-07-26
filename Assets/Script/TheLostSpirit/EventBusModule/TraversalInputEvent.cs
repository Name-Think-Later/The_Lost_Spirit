namespace Script.TheLostSpirit.EventBusModule {
    public class TraversalInputEvent : DomainEvent {
        public class PerformedEvent : TraversalInputEvent { }

        public class CancelEvent : TraversalInputEvent { }
    }
}