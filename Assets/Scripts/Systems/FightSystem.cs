using System;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class FightSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;

        readonly EcsWorld world = null;
        readonly EcsFilter<Hero, Clrd, Health> heroFilter = null;
        readonly EcsFilter<Enemy, Clrd, Trnsfrm> enemyFilter = null;
        readonly EcsFilter<Player> playerFilter = null;

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

                        heroFilter.GetEntity(hi).Get<DidPunch>();

                        ref var enemyTransform = ref enemyEntity.Get<Trnsfrm>();
                        CreateSound(gameConfig.heroFightSound, enemyTransform.value.position);
                        enemyTransform.value.DOMove(enemyTransform.value.position + Vector3.up * 0.3f, 0.2f).SetLoops(2, LoopType.Yoyo); // visual feedback on strikes

                        hero.lastHitTime = Time.time;
                        if (enemyHealth.value == 0) {
                            hero.state = HeroState.Roam;
                            ref var enemy = ref enemyEntity.Get<Enemy>();
                            enemy.state = EnemyState.Death;
                            CreateSound(gameConfig.enemyDeathSound, enemyTransform.value.position);

                            foreach (var pi in playerFilter) {
                                SpeechUtils.Add(playerFilter.GetEntity(pi), new [] { "And stay dead!", "Monsters, go home!" }, chance : 0.5f, TTL : 1f);
                            }

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

                ref var enemyTransform = ref enemyFilter.GetEntity(ei).Get<Trnsfrm>();
                CreateSound(gameConfig.enemyFightSound, enemyTransform.value.position);
                ref var heroTransform = ref heroFilter.GetEntity(hi).Get<Trnsfrm>();
                heroTransform.value.DOMove(heroTransform.value.position + Vector3.down * 0.3f, 0.2f).SetLoops(2, LoopType.Yoyo); // visual feedback on strikes

                // SpeechUtils.Add(heroFilter.GetEntity(hi), new [] { "OUCH", "OI", "MOMMY" }, 0.8f);

                foreach (var pi in playerFilter) {
                    SpeechUtils.Add(playerFilter.GetEntity(pi), new [] { "Watch out!", "Hey, watch out!", "Stupid monster!" }, chance : 0.1f, TTL : 1f);
                }
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

        void CreateSound(AudioClip clip, Vector3 position) {
            SoundUtils.Create(world, clip, position);
        }

    }
}