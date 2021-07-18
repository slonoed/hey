using System.Collections.Generic;
using Leopotam.Ecs;

namespace YANTH {
    // This system does 2 main things
    // First it add Zone component to entities STAYING in a zone
    //   entity can be in multiple zone in the same time
    //
    // Second it adds ZoneEnter / ZoneExit components when entitey entering / leaving
    // These components are only exist for one game loop run
    sealed class ZoneSystem : IEcsRunSystem {
        readonly EcsFilter<Zone, Clrd> zoneFilter = null;
        readonly EcsFilter<ZoneTarget, Clrd> targetFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var ti in targetFilter) {
                ref var targetEntity = ref targetFilter.GetEntity(ti);
                ref var target = ref targetFilter.Get1(ti);
                ref var targetCollider = ref targetFilter.Get2(ti);

                target.previousTags = target.tags;
                target.tags = new HashSet<string>();

                foreach (var zi in zoneFilter) {
                    ref var zoneCollider = ref zoneFilter.Get2(zi);

                    if (zoneCollider.value.IsTouching(targetCollider.value)) {
                        ref var zone = ref zoneFilter.Get1(zi);
                        target.tags.Add(zone.tag);
                    }
                }

                var(added, removed) = SetUtils.Difference(target.previousTags, target.tags);
                if (added.Count > 0) {
                    ref var zoneEnter = ref targetEntity.Get<ZoneEnter>();
                    zoneEnter.tags = added;
                } else {
                    targetEntity.Del<ZoneEnter>();
                }

                if (removed.Count > 0) {
                    ref var zoneExit = ref targetEntity.Get<ZoneExit>();
                    zoneExit.tags = removed;
                } else {
                    targetEntity.Del<ZoneExit>();
                }
            }
        }
    }
}