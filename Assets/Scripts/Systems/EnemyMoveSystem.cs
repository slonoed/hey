using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class EnemyMoveSystem : IEcsRunSystem {
        readonly EcsFilter<Enemy, Trnsfrm> enemiesFilter = null;
        readonly EcsFilter<Hero, Trnsfrm> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var heroTransform = ref heroFilter.Get2(hi);

                foreach (var ei in enemiesFilter) {
                    ref var enemy = ref enemiesFilter.Get1(ei);
                    ref var enemyTransform = ref enemiesFilter.Get2(ei);

                    var step = enemy.speed * Time.deltaTime;
                    enemyTransform.value.position = Vector3.MoveTowards(enemyTransform.value.position, heroTransform.value.position, step);
                    Lg.Log("Moving enemy", enemyTransform.value.position.x);
                }

            }
        }
    }
}