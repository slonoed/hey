using Leopotam.Ecs;

namespace YANTH {
    sealed class HeroDeathSystem : IEcsRunSystem {
        readonly GameRefs gameRefs = null;
        readonly EcsFilter<Hero, Health> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var health = ref heroFilter.Get2(hi);
                if (health.value <= 0) {
                    gameRefs.uiManager.RunAction("openDeath");
                }
            }
        }
    }
}