namespace TheLostSpirit.Identity.EntityID
{
    public record NodeID : RuntimeID<NodeID>
    {
        static int _index = 1;
        public int Index { get; } = _index++;
    }
}