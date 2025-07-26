namespace Script.TheLostSpirit.EventBusModule {
    public class TraversalInputEvent : DomainEvent {
        public TraversalInputEvent(int index) {
            Index = index;
        }

        public int Index { get; }

        public class PerformedEvent : TraversalInputEvent {
            public PerformedEvent(int index) : base(index) { }
        }

        public class CancelEvent : TraversalInputEvent {
            public CancelEvent(int index) : base(index) { }
        }
    }

    public class CircuitTraversalEvent : DomainEvent {
        public CircuitTraversalEvent(int index) {
            Index = index;
        }

        public int Index { get; }
    }
}