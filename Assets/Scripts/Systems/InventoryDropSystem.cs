using System;
using DG.Tweening;
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

                    var touching = playerCollider.value.IsTouching(heroCollider.value);
                    var keyDown = Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0);

                    ref var playerEntity = ref playerFilter.GetEntity(pi);
                    ref var heroEntity = ref heroFilter.GetEntity(hi);

                    if (touching && keyDown) {
                        ref var receiver = ref heroEntity.Get<DropReceiver>();
                        ref var producer = ref playerEntity.Get<DropProducer>();
                        producer.receiver = heroEntity;
                    } else {
                        playerEntity.Del<DropProducer>();
                        heroEntity.Del<DropReceiver>();
                    }
                }
            }
        }

        // void IEcsRunSystem.Run() {
        //     foreach (var pi in playerFilter) {
        //         ref var playerCollider = ref playerFilter.Get2(pi);
        //         foreach (var hi in heroFilter) {
        //             ref var heroCollider = ref heroFilter.Get2(hi);
        //             ref var heroTransform = ref heroFilter.GetEntity(hi).Get<Trnsfrm>();
        //             ref var inventory = ref playerFilter.Get3(pi);

        //             if (playerCollider.value.IsTouching(heroCollider.value) && inventory.items.Length > 0) {
        //                 if (!DOTween.IsTweening(heroTransform.value)) {
        //                     heroTransform.value.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.3f); // zoom hero
        //                 }

        //                 if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0)) {
        //                     TransferItems(pi, hi);
        //                     CreateSound(gameConfig.heroInventorySound);
        //                     SpeechUtils.Add(playerFilter.GetEntity(pi), "Take this!", 1f);
        //                 }
        //             } else if (!DOTween.IsTweening(heroTransform.value)) {
        //                 heroTransform.value.DOScale(new Vector3(1f, 1f, 1f), 0.3f); // unzoom hero
        //             }
        //         }
        //     }

        // }
    }
}