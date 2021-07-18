using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class HeroInitSystem : IEcsInitSystem {
        readonly EcsWorld world = null;
        readonly GameConfigSO gameConfig = null;
        readonly Cinemachine.CinemachineVirtualCamera virtualCamera = null;

        public void Init() {
            var heroEntity = world.NewEntity();
            var go = GameObject.Instantiate(gameConfig.heroPrefab);

            ref var transform = ref heroEntity.Get<Trnsfrm>();
            transform.value = go.transform;

            ref var hero = ref heroEntity.Get<Hero>();
            hero.state = HeroState.Roam;

            ref var animator = ref heroEntity.Get<Anmtr>();
            animator.value = go.GetComponent<Animator>();
            if (animator.value == null) {
                Lg.Warn("No Animator component on Hero prefab");
            }

            virtualCamera.Follow = go.transform;
        }
    }
}