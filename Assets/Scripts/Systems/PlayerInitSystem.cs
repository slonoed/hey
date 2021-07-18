using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class PlayerInitSystem : IEcsInitSystem {
        readonly EcsWorld world = null;
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Hero, Trnsfrm> heroFilter = null;
        readonly GameRefs gameRefs;

        public void Init() {
            foreach (var hi in heroFilter) {
                ref var t = ref heroFilter.Get2(hi);
                CreatePlayer(t);
            }
        }

        void CreatePlayer(Trnsfrm heroTransform) {
            var entity = world.NewEntity();

            entity.Get<Player>();

            var position = heroTransform.value.position + Vector3.up * 10;
            // var position = gameRefs.heroStartPoint.position;
            var go = GameObject.Instantiate(gameConfig.playerPrefab, position, Quaternion.identity);

            ref var transform = ref entity.Get<Trnsfrm>();
            transform.value = go.transform;

            ref var cldr = ref entity.Get<Clrd>();
            cldr.value = go.GetComponent<Collider2D>();
            if (cldr.value == null) {
                Lg.Warn("Player prefab should have Collider2D attached");
            }

            ref var inventory = ref entity.Get<Inventory>();
            inventory.size = gameConfig.playerInventorySize;
            inventory.items = new ResourceType[inventory.size];

            SpeechUtils.Add(entity, "HEY! Use keyboard to move!", 1f, TTL : 5f);
        }
    }
}