using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class HeroMovementSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;

        readonly EcsFilter<Hero, Trnsfrm>.Exclude<DropReceiver> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hfi in heroFilter) {
                ref var hero = ref heroFilter.Get1(hfi);
                ref var transform = ref heroFilter.Get2(hfi);

                if (hero.state == HeroState.Roam) {
                    transform.value.position += Vector3.up * Time.deltaTime * gameConfig.heroSpeed;

                    if (transform.value.position.y % 20 >= 5f && transform.value.position.y % 20 < 10f) // going left
                        transform.value.position += Vector3.left * Time.deltaTime * gameConfig.heroSpeed / 2f;
                    if (transform.value.position.y % 20 >= 15f && transform.value.position.y % 20 < 20f) // going right
                        transform.value.position += Vector3.right * Time.deltaTime * gameConfig.heroSpeed / 2f;
                }
            }
        }
    }
}