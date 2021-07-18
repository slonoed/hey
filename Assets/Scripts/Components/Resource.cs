namespace YANTH {
    public enum ResourceType {
        Empty = 0,
        Coin,
        Herb
    }

    struct Resource {
        public ResourceType type;
    }
}