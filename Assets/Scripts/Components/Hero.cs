namespace YANTH {
    enum HeroState {
        Invalid = 0,
        Roam,
        Fight,
        Wait,
        Death,
    }

    struct Hero {
        public HeroState state;
    }
}