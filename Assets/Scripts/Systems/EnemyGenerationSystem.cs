using Leopotam.Ecs;

namespace YANTH {
    sealed class EnemyGenerationSystem : IEcsRunSystem {
        // List of enemy objects to create enemies
        readonly EcsFilter<EnemyPrefab> enemies = null;

        void IEcsRunSystem.Run() {
            foreach (var ei in enemies) {
                ref var prefabValue = ref enemies.Get1(ei);
                ref var entity = ref enemies.GetEntity(ei);

                // Creating all components for enemy
                ref var health = ref entity.Get<Health>();
                health.max = prefabValue.maxHP;
                health.value = prefabValue.maxHP;
                ref var enemy = ref entity.Get<Enemy>();
                enemy.name = "Boris";

                // Remove initial prefab to avoid triggering code again
                entity.Del<EnemyPrefab>();
            }
        }
    }
}