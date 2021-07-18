using System;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class FightSystem : IEcsRunSystem {
        readonly EcsFilter<Hero, Clrd, Health> heroFilter = null;
        readonly EcsFilter<Enemy, Clrd> enemyFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var heroCollider = ref heroFilter.Get2(hi);
                foreach (var ei in enemyFilter) {
                    ref var enemyCollider = ref enemyFilter.Get2(ei);

                    if (enemyCollider.value.IsTouching(heroCollider.value)) {
                        ProcessFight(hi, ei);
                    }

                }
            }
        }

        void ProcessFight(int hi, int ei) {
            ref var enemy = ref enemyFilter.Get1(ei);
            enemy.state = EnemyState.Fight;
            ref var hero = ref heroFilter.Get1(hi);
            hero.state = HeroState.Fight;

            // Enemy hits player
            if (enemy.lastHitTime < Time.time - 2f) {
                ref var heroHealth = ref heroFilter.Get3(hi);
                heroHealth.value = Math.Max(0, heroHealth.value - enemy.attack);
                enemy.lastHitTime = Time.time;
            }
        }
    }
}