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
        public static void Add(in EcsEntity entity, string text, float chance = 1f, float TTL = 1.5f, bool overwrite = false, bool nonEssential = false) {
            if (nonEssential && IsHeroInNoSpeechZone(entity)) {
                return;
            }

            if (Random.value > chance)
                return;

            ref var speech = ref entity.Get<Speech>();
            if (!overwrite && speech.TTL > 0) {
                return;
            }
            speech.text = text;
            speech.TTL = TTL;
        }

        public static void Add(in EcsEntity entity, string[] lines, float chance = 1f, float TTL = 1.5f, bool overwrite = false, bool nonEssential = false) {
            if (nonEssential && IsHeroInNoSpeechZone(entity)) {
                return;
            }
            var idx = UnityEngine.Random.Range(0, lines.Length);
            Add(entity, lines[idx], chance, TTL, overwrite);
        }

        // This is a very hacky and non performant approach
        static bool IsHeroInNoSpeechZone(in EcsEntity entity) {
            var filter = entity.GetInternalWorld().GetFilter(typeof(EcsFilter<ZoneTarget, Hero>));
            foreach (var hi in filter) {
                var zone = filter.GetEntity(hi).Get<ZoneTarget>();
                if (zone.tags.Contains("noRandomSpeech")) {
                    return true;
                }
            }

            return false;
        }
    }
}