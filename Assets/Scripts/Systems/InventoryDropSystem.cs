using System;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class InventoryDropSystem : IEcsRunSystem {
        readonly EcsFilter<Hero, Clrd, Health, Trnsfrm> heroFilter = null;
        readonly EcsFilter<Player, Clrd, Inventory> playerFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                ref var playerCollider = ref playerFilter.Get2(pi);
                foreach (var hi in heroFilter) {
                    ref var heroCollider = ref heroFilter.Get2(hi);

                    var touching = playerCollider.value.IsTouching(heroCollider.value);
                    var keyDown = Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl) ||
                        Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0);

                    ref var playerEntity = ref playerFilter.GetEntity(pi);
                    ref var inventory = ref playerFilter.Get3(pi);
                    ref var heroEntity = ref heroFilter.GetEntity(hi);

                    if (touching && keyDown) {
                        ref var receiver = ref heroEntity.Get<DropReceiver>();
                        ref var producer = ref playerEntity.Get<DropProducer>();
                        producer.receiver = heroEntity;
                    } else {
                        playerEntity.Del<DropProducer>();
                        heroEntity.Del<DropReceiver>();

                        ref var hero = ref heroFilter.Get1(hi);
                        if (touching && hero.wallet <= 3 && !InventoryUtils.IsEmpty(inventory)) {
                            SpeechUtils.Add(playerEntity, "PRESS 'E' OR 'SPACE' TO UNLOAD!", 1f, 1f, overwrite : false);
                        }
                    }
                }
            }
        }
    }
}