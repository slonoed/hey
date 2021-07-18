using System;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
  sealed class ZoneCreationSystem : IEcsRunSystem {
    readonly EcsFilter<ZonePrefab> prefabFilter = null;

    void IEcsRunSystem.Run() {
      foreach (var pi in prefabFilter) {
        ref var prefab = ref prefabFilter.Get1(pi);
        ref var entity = ref prefabFilter.GetEntity(pi);

        ref var zone = ref entity.Get<Zone>();
        zone.tag = prefab.tag;

        ref var collider = ref entity.Get<Clrd>();
        collider.value = prefab.gameObject.GetComponent<Collider2D>();
        if (collider.value == null) {
          Lg.Warn("No collider on zone prefab", zone.tag);
        }
      }
    }
  }
}