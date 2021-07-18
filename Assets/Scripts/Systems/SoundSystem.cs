using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class SoundSystem : IEcsRunSystem {
        readonly EcsFilter<Sound> soundFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var si in soundFilter) {
                ref var sound = ref soundFilter.Get1(si);

                AudioSource.PlayClipAtPoint(sound.clip, sound.position);
                // AudioSource.Play(sound.clip);

                soundFilter.GetEntity(si).Del<Sound>();
            }
        }
    }
}