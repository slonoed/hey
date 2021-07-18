using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class EnemyMoveSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Enemy, Trnsfrm> enemiesFilter = null;
        readonly EcsFilter<Hero, Trnsfrm> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var heroTransform = ref heroFilter.Get2(hi);

                foreach (var ei in enemiesFilter) {
                    ref var enemy = ref enemiesFilter.Get1(ei);
                    ref var enemyTransform = ref enemiesFilter.Get2(ei);

                    var distance = Vector3.Distance(enemyTransform.value.position, heroTransform.value.position);
                    if (distance < gameConfig.roamDistance && enemy.state == EnemyState.Wait) {
                        enemy.state = EnemyState.Roam;
                    }

                    if (enemy.state == EnemyState.Roam) {
                        var step = enemy.speed * Time.deltaTime;
                        enemyTransform.value.position = Vector3.MoveTowards(enemyTransform.value.position, heroTransform.value.position, step);
                    }
                }
            }
        }
    }
}