using Leopotam.Ecs;

namespace YANTH {
    public enum HeroState {
        Invalid = 0,
        Roam,
        Fight,
        Wait,
        Death,
    }

    public struct Hero {
        public HeroState state;
        public int wallet;
        public EcsEntity targetEnemy;
        public float lastHitTime;
    }
}