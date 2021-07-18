using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    // Responsible for player collecting resources from the ground
    sealed class ResourceGatheringSystem : IEcsRunSystem {
        readonly EcsWorld world = null;
        readonly GameConfigSO gameConfig = null;

        readonly EcsFilter<Player, Clrd, Inventory, Trnsfrm> playerFilter = null;
        // Grab resources which are not collected yet
        readonly EcsFilter<Resource, Clrd, Trnsfrm>.Exclude<ResourceCollected> resourceFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                ref var inventory = ref playerFilter.Get3(pi);
                // Only collect resources when there is a space in inventory
                if (IsInventoryHasSpace(inventory)) {
                    foreach (var ri in resourceFilter) {
                        CheckCollection(pi, ri);
                    }
                }
            }
        }

        void CheckCollection(int pi, int ri) {
            ref var playerCldr = ref playerFilter.Get2(pi);
            ref var resourceCldr = ref resourceFilter.Get2(ri);

            if (resourceCldr.value.IsTouching(playerCldr.value)) {
                Collect(pi, ri);
            }
        }

        void Collect(int pi, int ri) {
            ref var resource = ref resourceFilter.Get1(ri);
            var type = resource.type;
            ref var inventory = ref playerFilter.Get3(pi);
            ref var pt = ref playerFilter.Get4(pi);
            var playerPosition = pt.value.position;
            var entity = resourceFilter.GetEntity(ri);

            AddToInventory(inventory.items, type);
            CreateSound(type, playerPosition);

            // Mark that resource is already collected by player and no need to react next time
            entity.Get<ResourceCollected>();

            // Grab transform component
            ref var transform = ref resourceFilter.Get3(ri);
            // Add tween on transform component
            transform.value.DOScale(new Vector3(0f, 0f, 0f), 0.3f);
            transform.value.DOMove(pt.value.position, 0.3f).OnComplete(() => {
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

        void CreateSound(ResourceType type, Vector3 position) {
            var clip = ResourceSound(type);
            if (clip == null) {
                Lg.Warn("Resource doesn't have sound", type.ToString());
                return;
            }

            SoundUtils.Create(world, clip, position);
        }

        AudioClip ResourceSound(ResourceType type) {
            switch (type) {
                case ResourceType.Coin:
                    return gameConfig.coinCollectionSound;
                case ResourceType.Herb:
                    return gameConfig.herbCollectionSound;
                default:
                    return null;
            }
        }

        bool IsInventoryHasSpace(Inventory inventory) {
            foreach (var item in inventory.items) {
                if (item == ResourceType.Empty) {
                    return true;
                }
            }
            return false;
        }

    }
}