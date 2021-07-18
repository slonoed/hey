using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    // Responsible for player collecting resources from the ground
    sealed class ResourceGatheringSystem : IEcsRunSystem {
        readonly EcsFilter<Player, Clrd, Inventory> playerFilter = null;
        readonly EcsFilter<Resource, Clrd, Trnsfrm> resourceFilter = null;

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
                HandleCollision(pi, ri);
            }
        }

        void HandleCollision(int pi, int ri) {
            ref var resource = ref resourceFilter.Get1(ri);
            ref var inventory = ref playerFilter.Get3(pi);

            AddToInventory(inventory.items, resource.type);

            resourceFilter.GetEntity(ri).Destroy();
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