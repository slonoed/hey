using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class HeroMovementSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Hero, Trnsfrm> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hfi in heroFilter) {
                ref var transform = ref heroFilter.Get2(hfi);
                transform.value.position += Vector3.up * Time.deltaTime * gameConfig.heroSpeed;
            }
        }
    }
}