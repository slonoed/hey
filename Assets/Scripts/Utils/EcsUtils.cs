// This file contains generic utils to help creating components and entities
// It purpose is to keep components code as simple as possible but still provide some reusable helpers to systems
// If grows to large split to separte utils
//
// IMPORTANT
// Do not modify any outer state. Function should be pure and only modify arguments when needed.
// Keep everything static.
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    public static class SoundUtils {
        // Creates entity and adds Sound to it
        public static void Create(EcsWorld world, AudioClip clip, Vector3 position) {
            // Note: add "out" entity and component if needed

            ref var sound = ref world.NewEntity().Get<Sound>();
            sound.position = position;
            sound.clip = clip;
        }
    }

    public static class SpeechUtils {
        // Adds/updates speech component on entity
        // When overwrite is true it will replace existing text
        public static void Add(in EcsEntity entity, string text, float TTL, bool overwrite = true) {
            ref var speech = ref entity.Get<Speech>();
            if (!overwrite && speech.TTL > 0) {
                return;
            }
            speech.text = text;
            speech.TTL = TTL;
        }
    }
}