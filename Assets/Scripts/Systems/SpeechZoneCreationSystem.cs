using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class SpeechZoneCreationSystem : IEcsRunSystem {
        readonly EcsFilter<SpeechZonePrefab> prefabsFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in prefabsFilter) {
                ref var entity = ref prefabsFilter.GetEntity(pi);
                ref var prefab = ref prefabsFilter.Get1(pi);

                ref var zone = ref entity.Get<SpeechZone>();
                zone.lines = prefab.lines;

                ref var collider = ref entity.Get<Clrd>();
                collider.value = prefab.gameObject.GetComponent<Collider2D>();
                if (collider.value == null) {
                    Lg.Warn("speech zone should have collider");
                }
            }
        }
    }
}