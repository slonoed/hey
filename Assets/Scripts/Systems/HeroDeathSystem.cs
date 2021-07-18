using Leopotam.Ecs;

namespace YANTH {
    sealed class HeroDeathSystem : IEcsRunSystem {
        readonly GameRefs gameRefs = null;
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Hero, Health> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var health = ref heroFilter.Get2(hi);
                if (health.value <= 0) {
                    health.value = 100 * gameConfig.deathHealthRestoration / health.max;

                    ref var hero = ref heroFilter.Get1(hi);

                    if (hero.wallet > 0) {
                        hero.particleSystem.Play();
                    }
                    hero.wallet = hero.wallet * (100 - gameConfig.deathMoneyLost) / 100;

                    SpeechUtils.Add(hero.player, gameConfig.sayHeroDeath, 1, 1.5f, true);
                }
            }
        }
    }
}