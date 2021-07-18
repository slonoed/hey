using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    // Responsible for player collecting resources from the ground
    sealed class ResourceGatheringSystem : IEcsRunSystem {
        readonly EcsFilter<Player, Clrd> playerFilter = null;
        readonly EcsFilter<Resource, Clrd, Trnsfrm> resourceFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                ref var collider = ref playerFilter.Get2(pi);
                CheckResourceCollision(collider.value);
            }
        }

        void CheckResourceCollision(Collider2D playerCollider) {
            foreach (var ri in resourceFilter) {
                ref var rCollider = ref resourceFilter.Get2(ri);
                if (rCollider.value.IsTouching(playerCollider)) {
                    HandleCollision(ri);
                }

            }
        }

        void HandleCollision(int ri) {
            // TODO instead of removing add mark to remove to run animations, etc.
            resourceFilter.GetEntity(ri).Destroy();
        }
    }
}