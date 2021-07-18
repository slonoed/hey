using Leopotam.Ecs;
using UnityEngine;

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
        public int level;
        public EcsEntity player;
        public ParticleSystem particleSystem;
    }
}