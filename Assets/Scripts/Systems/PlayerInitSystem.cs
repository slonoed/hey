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
                ref var hero = ref heroFilter.Get1(hi);
                var playerEntity = CreatePlayer(t, hi);
                hero.player = playerEntity;
            }
        }

        EcsEntity CreatePlayer(Trnsfrm heroTransform, int hi) {
            var entity = world.NewEntity();

            ref var player = ref entity.Get<Player>();

            var position = heroTransform.value.position + Vector3.up * 3 + Vector3.right * 2;
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

            return entity;
        }
    }
}