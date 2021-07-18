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
            transform.value.DOMove(playerPosition, 0.4f).OnComplete(() => {
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

            ref var sound = ref world.NewEntity().Get<Sound>();
            sound.position = position;
            sound.clip = clip;
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
    }
}