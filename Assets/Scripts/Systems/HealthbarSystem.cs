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

            foreach (var hi in healthbarFilter) {
                UpdateView(hi);
            }
        }

        void CreateHealthbar(int ti) {
            ref var entity = ref targetFilter.GetEntity(ti);
            ref var targetTransform = ref entity.Get<Trnsfrm>();
            ref var healthbar = ref entity.Get<Healthbar>();

            var go = GameObject.Instantiate(gameConfig.healthbarPrefab, targetTransform.value);
            var foreground = Traverse.FindChildWithName(go, "Foreground");
            if (foreground == null) {
                Lg.Warn("Can't find 'Foreground' component on Healthbar prefab");
                return;
            }

            healthbar.rect = foreground.GetComponent<RectTransform>();
            if (healthbar.rect == null) {
                Lg.Warn("Can't find RectTransform on Healthbar prefab");
                return;
            }
        }

        void UpdateView(int hi) {
            ref var healthbar = ref healthbarFilter.Get1(hi);
            ref var health = ref healthbarFilter.Get2(hi);

            // Health value [0;1]
            var value = (float) health.value / (float) health.max;

            healthbar.rect.anchorMax = new Vector2(value, 1);
        }
    }
}