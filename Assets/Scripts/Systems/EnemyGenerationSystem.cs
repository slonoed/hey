using Leopotam.Ecs;

namespace YANTH {
    sealed class EnemyGenerationSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        // List of enemy objects to create enemies
        readonly EcsFilter<EnemyPrefab> enemiesFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var ei in enemiesFilter) {
                ref var prefabValue = ref enemiesFilter.Get1(ei);
                ref var entity = ref enemiesFilter.GetEntity(ei);

                // Creating all components for enemy
                ref var transform = ref entity.Get<Trnsfrm>();
                transform.value = prefabValue.transform;

                ref var health = ref entity.Get<Health>();
                health.max = prefabValue.maxHP;
                if (health.max == 0) {
                    health.max = gameConfig.enemyMaxHPDefault;
                }
                health.value = health.max;

                ref var enemy = ref entity.Get<Enemy>();
                enemy.name = "Boris";
                enemy.speed = prefabValue.speed;
                if (enemy.speed == 0) {
                    enemy.speed = gameConfig.enemySpeedDefault;
                }

                // Remove initial prefab to avoid triggering code again
                entity.Del<EnemyPrefab>();
            }
        }
    }
}