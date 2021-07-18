using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    // Responsible for player collecting resources from the ground
    sealed class ResourceGatheringSystem : IEcsRunSystem {
        readonly EcsFilter<Player, Clrd, Inventory> playerFilter = null;
        // Grab resources which are not collected yet
        readonly EcsFilter<Resource, Clrd, Trnsfrm>.Exclude<ResourceCollected> resourceFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                foreach (var ri in resourceFilter) {
                    CheckResourceCollision(pi, ri);
                }
            }
        }

        void CheckResourceCollision(int pi, int ri) {
            ref var playerCldr = ref playerFilter.Get2(pi);
            ref var resourceCldr = ref resourceFilter.Get2(ri);
            if (resourceCldr.value.IsTouching(playerCldr.value)) {
                Collect(pi, ri);
            }
        }

        void Collect(int pi, int ri) {
            ref var resource = ref resourceFilter.Get1(ri);
            ref var inventory = ref playerFilter.Get3(pi);

            AddToInventory(inventory.items, resource.type);

            var entity = resourceFilter.GetEntity(ri);

            // Mark that resource is already collected by player and no need to react next time
            entity.Get<ResourceCollected>();

            // Grab transform component
            ref var transform = ref resourceFilter.Get3(ri);
            // Add tween on transform component
            transform.value.DOShakePosition(0.3f).OnComplete(() => {
                // When tween done mark this entity to be destroyed
                entity.Get<DestroyMark>();

                // TODO slonoed: ensure memory integrity is not broken if entity was already removed before
                // maybe it needs IsAlive here
            });
        }

        void AddToInventory(ResourceType[] items, ResourceType item) {
            for (int i = 0; i < items.Length; i++) {
                if (items[i] == ResourceType.Empty) {
                    items[i] = item;
                    return;
                }
            }
        }
    }
}