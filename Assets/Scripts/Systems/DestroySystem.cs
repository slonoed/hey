using Leopotam.Ecs;

namespace YANTH {
    sealed class DestroySystem : IEcsRunSystem {
        readonly EcsFilter<DestroyMark> entitiesForDestroyFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var efdi in entitiesForDestroyFilter) {
                entitiesForDestroyFilter.GetEntity(efdi).Destroy();
            }
        }
    }
}