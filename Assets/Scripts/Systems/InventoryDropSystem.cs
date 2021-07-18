using DG.Tweening;
using System;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class InventoryDropSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;

        readonly EcsWorld world = null;
        readonly EcsFilter<Hero, Clrd, Health, Trnsfrm> heroFilter = null;
        readonly EcsFilter<Player, Clrd, Inventory> playerFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                ref var playerCollider = ref playerFilter.Get2(pi);
                foreach (var hi in heroFilter) {
                    ref var heroCollider = ref heroFilter.Get2(hi);
                    ref var heroTransform = ref heroFilter.GetEntity(hi).Get<Trnsfrm>();
                    ref var inventory = ref playerFilter.Get3(pi);
                    
                    if (playerCollider.value.IsTouching(heroCollider.value) && inventory.items.Length > 0) {
                        if (!DOTween.IsTweening(heroTransform.value))
                            heroTransform.value.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.3f); // zoom hero

                        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0)) {
                            TransferItems(pi, hi);
                            CreateSound(gameConfig.heroInventorySound);
                        }
                    }
                    else if (!DOTween.IsTweening(heroTransform.value)) {
                        heroTransform.value.DOScale(new Vector3(1f, 1f, 1f), 0.3f); // unzoom hero
                    }
                }
            }


        }

        // Apply player inventory to hero
        void TransferItems(int pi, int hi) {
            ref var invetory = ref playerFilter.Get3(pi);
            ref var hero = ref heroFilter.Get1(hi);
            ref var health = ref heroFilter.Get3(hi);

            var coins = 0;
            var herbs = 0;

            for (int i = 0; i < invetory.items.Length; i++) {
                var item = invetory.items[i];
                invetory.items[i] = ResourceType.Empty;
                if (item == ResourceType.Coin) {
                    coins++;
                } else if (item == ResourceType.Herb) {
                    herbs++;
                }
            }

            hero.wallet += coins;
            health.value = Math.Min(health.value + herbs * gameConfig.herbHealingFactor, health.max);
        }

        void CreateSound(AudioClip clip, Vector3 position = new Vector3()) {
            ref var sound = ref world.NewEntity().Get<Sound>();
            sound.position = position;
            sound.clip = clip;
        }
    }

}