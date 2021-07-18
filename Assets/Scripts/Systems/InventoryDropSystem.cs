using System;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class InventoryDropSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;

        readonly EcsFilter<Hero, Clrd, Health> heroFilter = null;
        readonly EcsFilter<Player, Clrd, Inventory> playerFilter = null;

        void IEcsRunSystem.Run() {
            if (!Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Space)) {
                return;
            }

            foreach (var pi in playerFilter) {
                ref var playerCollider = ref playerFilter.Get2(pi);
                foreach (var hi in heroFilter) {
                    ref var heroCollider = ref heroFilter.Get2(hi);
                    if (playerCollider.value.IsTouching(heroCollider.value)) {
                        TransferItems(pi, hi);
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
    }
}