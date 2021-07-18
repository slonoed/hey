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

            heroEntity.Get<Hero>();

            virtualCamera.Follow = go.transform;
        }
    }
}