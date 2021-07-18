using Leopotam.Ecs;

namespace YANTH {
    sealed class LevelChangeSystem : IEcsRunSystem {
        readonly EcsFilter<Hero, ZoneEnter> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var zone = ref heroFilter.Get2(hi);
                if (zone.tags.Contains("levelEnd")) {
                    Lg.Log("new level");
                }
            }
        }
    }
}