namespace Script.TheLostSpirit.EventBusModule {
    public class TraversalInputEvent : DomainEvent {
        public TraversalInputEvent(int index, bool isPress) {
            Index   = index;
            IsPress = isPress;
        }

        public int Index { get; }
        public bool IsPress { get; }
    }
}