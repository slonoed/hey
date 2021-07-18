using System;
using Leopotam.Ecs;

namespace YANTH {
    // This system applies received items to hero
    sealed class HeroDropReceivingSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Hero, DropReceiver, Health, Trnsfrm> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var receiver = ref heroFilter.Get2(hi);

                while (receiver.incomming.Count > 0) {
                    var item = receiver.incomming.Dequeue();

                    if (item == ResourceType.Coin) {
                        ref var hero = ref heroFilter.Get1(hi);
                        hero.wallet++;
                        AnalyticsUtils.Emit("drop_coin");
                    }
                    if (item == ResourceType.Herb) {
                        ref var health = ref heroFilter.Get3(hi);
                        health.value = Math.Min(health.value + gameConfig.herbHealingFactor, health.max);
                        AnalyticsUtils.Emit("drop_herb");
                    }
                }
            }
        }
    }
}