using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class PlayerInitSystem : IEcsInitSystem {
        readonly EcsWorld world = null;
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Player> playerFilter = null;
        readonly EcsFilter<Hero, Trnsfrm> heroFilter = null;

        public void Init() {
            if (!playerFilter.IsEmpty()) {
                return;
            }

            if (heroFilter.IsEmpty()) {
                // Wait til hero exists
                return;
            }

            foreach (var hi in heroFilter) {
                ref var t = ref heroFilter.Get2(hi);
                CreatePlayer(t);
                // Only first existing hero
                return;
            }
        }

        void CreatePlayer(Trnsfrm heroTransform) {
            var entity = world.NewEntity();

            entity.Get<Player>();

            var position = heroTransform.value.position + Vector3.down * 2;
            var go = GameObject.Instantiate(gameConfig.playerPrefab, position, Quaternion.identity);

            ref var transform = ref entity.Get<Trnsfrm>();
            transform.value = go.transform;
        }
    }
}