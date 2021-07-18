using Leopotam.Ecs;
using UnityEngine;

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
                enemy.attack = prefabValue.attack;
                if (enemy.attack == 0) {
                    enemy.attack = gameConfig.enemyAttackDefault;
                }
                enemy.attackDelay = prefabValue.attackDelay;
                if (enemy.attackDelay == 0) {
                    enemy.attackDelay = gameConfig.enemyAttachDelayDefault;
                }

                ref var collider = ref entity.Get<Clrd>();
                collider.value = transform.value.gameObject.GetComponent<Collider2D>();
                if (collider.value == null) {
                    Lg.Warn("Enemy should have collider: ", enemy.name);
                }

                // Remove initial prefab to avoid triggering code again
                entity.Del<EnemyPrefab>();
            }
        }
    }
}