using Leopotam.Ecs;

namespace YANTH {
    sealed class HeroSpeechZoneSystem : IEcsRunSystem {
        readonly EcsFilter<Hero, Clrd> heroFilter = null;
        readonly EcsFilter<SpeechZone, Clrd>.Exclude<SpechZoneVisited> zoneFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var heroCollider = ref heroFilter.Get2(hi);
                ref var heroEntity = ref heroFilter.GetEntity(hi);

                foreach (var zi in zoneFilter) {
                    ref var zoneCollider = ref zoneFilter.Get2(zi);
                    if (heroCollider.value.IsTouching(zoneCollider.value)) {
                        ref var zone = ref zoneFilter.Get1(zi);
                        SpeechUtils.Add(heroEntity, zone.lines);

                        zoneFilter.GetEntity(zi).Get<SpechZoneVisited>();
                    }
                }
            }
        }
    }
}