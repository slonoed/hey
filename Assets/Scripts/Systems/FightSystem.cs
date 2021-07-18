using System;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class FightSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;

        readonly EcsFilter<Hero, Clrd, Health> heroFilter = null;
        readonly EcsFilter<Enemy, Clrd> enemyFilter = null;

        void IEcsRunSystem.Run() {
            // TODO slonoed: this system is overloaded (logically and structurally)
            // Needs refactoring
            foreach (var hi in heroFilter) {
                ref var heroCollider = ref heroFilter.Get2(hi);
                ref var hero = ref heroFilter.Get1(hi);

                var hasTarget = HeroHasTarget(hi);
                if (!hasTarget) {
                    hero.targetEnemy = EcsEntity.Null;
                }

                foreach (var ei in enemyFilter) {
                    ref var enemy = ref enemyFilter.Get1(ei);
                    ref var enemyCollider = ref enemyFilter.Get2(ei);

                    if (enemyCollider.value.IsTouching(heroCollider.value) && enemy.state != EnemyState.Death) {
                        ProcessEnemyAttack(hi, ei);

                        if (!hasTarget) {
                            hero.targetEnemy = enemyFilter.GetEntity(ei);
                            hasTarget = true;
                        }
                    }
                }

                if (hasTarget) {
                    hero.state = HeroState.Fight;

                    // Hero attack enemy
                    if (hero.lastHitTime < Time.time - gameConfig.heroAttackDelay) {
                        var enemyEntity = hero.targetEnemy;
                        ref var enemyHealth = ref enemyEntity.Get<Health>();
                        enemyHealth.value = Math.Max(0, enemyHealth.value - gameConfig.attack);
                        hero.lastHitTime = Time.time;
                        if (enemyHealth.value == 0) {
                            hero.state = HeroState.Roam;
                            ref var enemy = ref enemyEntity.Get<Enemy>();
                            enemy.state = EnemyState.Death;
                            ref var enemyTransform = ref enemyEntity.Get<Trnsfrm>();

                            // Enemy death animation is here
                            enemyTransform.value.DOShakePosition(0.3f, 1f).OnComplete(() => {
                                if (enemyEntity.IsAlive()) {
                                    enemyEntity.Get<DestroyMark>();
                                }
                            });
                        }
                    }
                }
            }
        }

        void ProcessEnemyAttack(int hi, int ei) {
            ref var enemy = ref enemyFilter.Get1(ei);
            enemy.state = EnemyState.Fight;
            ref var hero = ref heroFilter.Get1(hi);
            // hero.state = HeroState.Fight;

            // Enemy hits player
            if (enemy.lastHitTime < Time.time - enemy.attackDelay) {
                ref var heroHealth = ref heroFilter.Get3(hi);
                heroHealth.value = Math.Max(0, heroHealth.value - enemy.attack);
                enemy.lastHitTime = Time.time;
            }
        }

        bool HeroHasTarget(int hi) {
            ref var hero = ref heroFilter.Get1(hi);
            if (hero.targetEnemy == EcsEntity.Null) {
                return false;
            }

            if (!hero.targetEnemy.IsAlive()) {
                return false;
            }

            ref var targetEnemy = ref hero.targetEnemy.Get<Enemy>();
            if (targetEnemy.state == EnemyState.Invalid) {
                return false;
            }
            if (targetEnemy.state == EnemyState.Death) {
                return false;
            }

            ref var heroCollider = ref heroFilter.Get2(hi);
            ref var enemyCollider = ref hero.targetEnemy.Get<Clrd>();
            if (enemyCollider.value == null) {
                return false;
            }

            if (!enemyCollider.value.IsTouching(heroCollider.value)) {
                return false;
            }

            return true;
        }
    }
}