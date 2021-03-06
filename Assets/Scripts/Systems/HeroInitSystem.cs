using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class HeroInitSystem : IEcsInitSystem {
        readonly EcsWorld world = null;
        readonly GameConfigSO gameConfig = null;
        readonly GameRefs gameRefs;

        public void Init() {
            var heroEntity = world.NewEntity();
            var go = GameObject.Instantiate(gameConfig.heroPrefab);

            heroEntity.Get<ZoneTarget>();

            ref var hero = ref heroEntity.Get<Hero>();
            hero.state = HeroState.Roam;
            hero.wallet = gameConfig.heroInitWallet;
            hero.level = 1;
            hero.particleSystem = go.GetComponentInChildren<ParticleSystem>();
            if (hero.particleSystem == null) {
                Lg.Warn("Hero should have particle system in children");
            }

            ref var transform = ref heroEntity.Get<Trnsfrm>();
            transform.value = go.transform;
            transform.value.position = gameRefs.levelStartPoints.GetChild(hero.level - 1).position;

            ref var health = ref heroEntity.Get<Health>();
            health.max = gameConfig.heroMaxHP;
            health.value = Mathf.Min(gameConfig.heroMaxHP, gameConfig.heroInitHP);

            ref var animator = ref heroEntity.Get<Anmtr>();
            animator.value = go.GetComponent<Animator>();
            if (animator.value == null) {
                Lg.Warn("No Animator component on Hero prefab");
            }

            ref var collider = ref heroEntity.Get<Clrd>();
            collider.value = go.GetComponent<Collider2D>();
            if (collider.value == null) {
                Lg.Warn("No Collider2D component on Hero prefab");
            }

            heroEntity.Get<CameraFollowTarget>();
        }
    }
}