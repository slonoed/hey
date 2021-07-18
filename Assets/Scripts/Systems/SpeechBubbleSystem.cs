using Leopotam.Ecs;
using TMPro;
using UnityEngine;

namespace YANTH {
    // System renders and controls speech bubbles on entities with transform
    sealed class SpeechBubbleSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Trnsfrm, Speech> speechFilter = null;

        readonly string goName = "SpeechBubbleCanvas";

        void IEcsRunSystem.Run() {
            foreach (var si in speechFilter) {
                ref var transform = ref speechFilter.Get1(si);
                ref var speech = ref speechFilter.Get2(si);

                // Get or create bubble canvas
                var bubbleCanvas = Traverse.FindChildWithName(transform.value.gameObject, goName);
                if (bubbleCanvas == null) {
                    bubbleCanvas = GameObject.Instantiate(gameConfig.speechBubblePrefab, transform.value);
                    bubbleCanvas.name = goName;
                }

                var label = GetBubbleLabel(bubbleCanvas);
                if (label != null) {
                    label.text = speech.text;
                }

                if (speech.TTL < 0) {
                    bubbleCanvas.SetActive(false);
                } else {
                    bubbleCanvas.SetActive(true);
                    speech.TTL -= Time.deltaTime;
                }
            }
        }

        TMP_Text GetBubbleLabel(GameObject bubble) {
            var labelGo = Traverse.FindChildWithName(bubble, "SpeechText");
            if (labelGo == null) {
                Lg.Warn("Speech bubble prefab should have TMP text with name 'SpeechText'");
                return null;
            }

            var label = labelGo.GetComponent<TMP_Text>();
            if (label == null) {
                Lg.Warn("Speech bubble prefab should have TMP text with name 'SpeechText'");
                return null;
            }

            return label;
        }

    }

}