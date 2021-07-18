using System;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace YANTH {
    sealed class InventoryRenderSystem : IEcsRunSystem {
        readonly GameRefs gameRefs;
        readonly GameConfigSO gameConfig = null;

        readonly EcsFilter<Inventory> inventoryFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var ii in inventoryFilter) {
                ref var inventory = ref inventoryFilter.Get1(ii);
                RenderInventory(inventory);
            }
        }

        void RenderInventory(Inventory inventory) {
            var inventoryTransform = gameRefs.inventoryPanel.transform;
            var panelsCount = inventoryTransform.childCount;
            var size = Math.Max(inventory.items.Length, panelsCount);

            // Loop body is pretty big but avoids multiple passes
            for (int i = 0; i < size; i++) {
                var panel = GetOrCreateItemPanel(i);
                var rect = panel.GetComponent<RectTransform>();
                // TODO slonoed: remove fixed size
                rect.anchoredPosition = new Vector2( 90 * (i-2), 0);
                
                if (i < inventory.items.Length) {
                    panel.gameObject.SetActive(true);
                    var image = Traverse.FindChildWithName(panel.gameObject, "Image").GetComponent<Image>();
                    UpdateImage(image, inventory.items[i]);
                } else {
                    panel.gameObject.SetActive(false);
                }
            }
        }

        Transform GetOrCreateItemPanel(int itemIndex) {
            var inventoryTransform = gameRefs.inventoryPanel.transform;
            var panelsCount = inventoryTransform.childCount;

            Transform panel;
            if (itemIndex < panelsCount) {
                panel = gameRefs.inventoryPanel.transform.GetChild(itemIndex);
            } else {
                var go = GameObject.Instantiate(gameConfig.inventoryItem, inventoryTransform);
                panel = go.transform;
            }

            return panel;
        }

        void UpdateImage(Image image, ResourceType item) {
            if (item == ResourceType.Coin) {
                image.color = Color.white;
                image.sprite = gameConfig.inventoryCoin;
            } else if (item == ResourceType.Herb) {
                image.color = Color.white;
                image.sprite = gameConfig.inventoryHerb;
            } else {
                image.sprite = null;
                image.color = new Color(0, 0, 0, 0);
            }

        }
    }
}