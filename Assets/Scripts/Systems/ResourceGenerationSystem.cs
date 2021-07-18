using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class ResourceGenerationSystem : IEcsRunSystem {
        readonly EcsWorld world = null;
        readonly GameConfigSO gameConfig = null;

        readonly EcsFilter<Hero, Trnsfrm> heroFilter = null;
        readonly EcsFilter<Resource, Trnsfrm> resourcesFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var transform = ref heroFilter.Get2(hi);
                GenerateResources(transform.value.position);

                CleanupResources(transform.value.position);
                // Handle only one hero
                return;
            }
        }

        void GenerateResources(Vector3 heroPosition) {
            var windowBottom = heroPosition.y + gameConfig.resourceGenerationDistance;
            var windowTop = windowBottom + 4f;

            var count = 0;

            foreach (var ri in resourcesFilter) {
                ref var transform = ref resourcesFilter.Get2(ri);
                var y = transform.value.position.y;

                if (Nums.IsBetween(windowBottom, y, windowTop)) {
                    count++;
                }
            }

            if (count < gameConfig.resourceDencity) {
                var pos = GetRandomResourcePosition(windowBottom, windowTop);
                CreateResouce(pos);
            }
        }

        void CreateResouce(Vector3 position) {
            var entity = world.NewEntity();

            ref var resource = ref entity.Get<Resource>();
            resource.type = Random.value > 0.5f ? ResourceType.Herb : ResourceType.Coin;

            var pref = resource.type == ResourceType.Coin ? gameConfig.coinPrefab : gameConfig.herbPrefab;

            var go = GameObject.Instantiate(pref, position, Quaternion.identity);

            ref var transform = ref entity.Get<Trnsfrm>();
            transform.value = go.transform;

            ref var collider = ref entity.Get<Clrd>();

            collider.value = go.GetComponent<Collider2D>();
            if (collider.value == null) {
                Lg.Warn("coin prefab should have collider");
            }
        }

        Vector3 GetRandomResourcePosition(float bottom, float top) {
            var x = Random.Range(-5f, 5f);
            var y = Random.Range((float) bottom, (float) top);
            return new Vector3(x, y, 0);
        }

        // Remove resources which are far behind
        void CleanupResources(Vector3 heroPosition) {
            foreach (var ri in resourcesFilter) {
                ref var transform = ref resourcesFilter.Get2(ri);
                var y = transform.value.position.y;

                if (y < heroPosition.y - 15f) {
                    RemoveResource(resourcesFilter.GetEntity(ri));
                }
            }
        }

        void RemoveResource(EcsEntity entity) {
            entity.Destroy();
        }
    }
}