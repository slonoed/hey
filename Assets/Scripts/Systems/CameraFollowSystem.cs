using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class CameraFollowSystem : IEcsRunSystem {
        readonly GameRefs gameRefs = null;
        readonly EcsFilter<CameraFollowTarget, Trnsfrm, ZoneTarget> targetFilter = null;

        void IEcsRunSystem.Run() {
            Transform follow = null;

            foreach (var ti in targetFilter) {
                ref var transform = ref targetFilter.Get2(ti);
                ref var zone = ref targetFilter.Get3(ti);
                if (!zone.tags.Contains("noCameraFollow")) {
                    follow = transform.value;
                }
            }

            if (gameRefs.virtualCamera.Follow != follow) {
                gameRefs.virtualCamera.Follow = follow;
            }
        }
    }
}