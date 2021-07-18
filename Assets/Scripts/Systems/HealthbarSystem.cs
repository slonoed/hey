using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class HealthbarSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;

        // Entities which don't have healthbar yet
        readonly EcsFilter<Trnsfrm, Health>.Exclude<Healthbar> targetFilter = null;
        // Entities with healthbar
        readonly EcsFilter<Healthbar, Health> healthbarFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var ti in targetFilter) {
                CreateHealthbar(ti);
            }
        }

        void CreateHealthbar(int ti) {
            ref var entity = ref targetFilter.GetEntity(ti);
            ref var targetTransform = ref entity.Get<Trnsfrm>();
            ref var healthbar = ref entity.Get<Healthbar>();

            var go = GameObject.Instantiate(gameConfig.healthbarPrefab, targetTransform.value);
        }
    }
}