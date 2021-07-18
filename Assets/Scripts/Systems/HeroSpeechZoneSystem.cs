using Leopotam.Ecs;
using System;

namespace YANTH {
    sealed class HeroSpeechZoneSystem : IEcsRunSystem {
        readonly EcsFilter<Hero, Clrd> heroFilter = null;
        readonly EcsFilter<Player> playerFilter = null;
        readonly EcsFilter<SpeechZone, Clrd>.Exclude<SpechZoneVisited> zoneFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var heroCollider = ref heroFilter.Get2(hi);
                ref var heroEntity = ref heroFilter.GetEntity(hi);

                foreach (var zi in zoneFilter) {
                    ref var zoneCollider = ref zoneFilter.Get2(zi);
                    if (heroCollider.value.IsTouching(zoneCollider.value)) {

                        ref var zone = ref zoneFilter.Get1(zi);
                        foreach (var pi in playerFilter) {
                            ref var playerEntity = ref playerFilter.GetEntity(pi);
                            
                            string[] uppercased = Array.ConvertAll(zone.lines, d => d.ToUpper());
                            SpeechUtils.Add(playerEntity, uppercased, chance: 1f, TTL: 3f, overwrite: true);
                        }

                        zoneFilter.GetEntity(zi).Get<SpechZoneVisited>();
                    }
                }
            }
        }
    }
}