using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    // Responsible for player collecting resources from the ground
    sealed class ResourceGatheringSystem : IEcsRunSystem {
        readonly EcsWorld world = null;
        readonly GameConfigSO gameConfig = null;
        readonly GameRefs gameRefs;

        readonly EcsFilter<Player, Clrd, Inventory, Trnsfrm> playerFilter = null;
        readonly EcsFilter<Hero, Clrd, Health, Trnsfrm> heroFilter = null;
        // Grab resources which are not collected yet
        readonly EcsFilter<Resource, Clrd, Trnsfrm>.Exclude<ResourceCollected> resourceFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                foreach (var ri in resourceFilter) {
                    CheckCollection(pi, ri);
                }
            }
        }

        void CheckCollection(int pi, int ri) {
            ref var playerCldr = ref playerFilter.Get2(pi);
            ref var resourceCldr = ref resourceFilter.Get2(ri);

            if (resourceCldr.value.IsTouching(playerCldr.value)) {
                ref var inventory = ref playerFilter.Get3(pi);
                if (InventoryUtils.HasSpace(inventory)) {
                    Collect(pi, ri);
                } else {
                    foreach (var hi in heroFilter) {
                        ref var hero = ref heroFilter.Get1(hi);
                        if (hero.wallet <= 12) {
                            ref var playerEntity = ref playerFilter.GetEntity(pi);
                            SpeechUtils.Add(playerEntity, new [] { "NO MORE SPACE!", "FLY TO HERO!" }, chance : 1f, TTL : 0.7f, overwrite : false, true);
                        }
                    }

                    if (!DOTween.IsTweening(gameRefs.inventoryPanel.transform))
                        gameRefs.inventoryPanel.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.2f).SetLoops(2, LoopType.Yoyo);
                }
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

            ref var playerEntity = ref playerFilter.GetEntity(pi);
            if (type == ResourceType.Herb)
                SpeechUtils.Add(playerEntity, new [] { "Yummy!", "Juicy!", "Tasty!", "Delicious!", "What a treat!", "Healthy!", "Healing flower!" }, chance : 0.3f, TTL : 1f, false, true);
            else
                SpeechUtils.Add(playerEntity, new [] { "Money!", "Coin!", "Shiny!", "Jingle!", "Loot!", "Rich!", "Ooh, money!" }, chance : 0.3f, TTL : 1f, false, true);

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
    }
}