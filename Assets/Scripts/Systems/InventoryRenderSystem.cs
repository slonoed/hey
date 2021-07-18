using Leopotam.Ecs;

namespace YANTH {
    sealed class InventoryRenderSystem : IEcsRunSystem {
        readonly GameRefs gameRefs;

        readonly EcsFilter<Inventory> inventoryFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var ii in inventoryFilter) {
                ref var inventory = ref inventoryFilter.Get1(ii);
                RenderInventory(inventory);
                // Only render one for now
                return;
            }
        }

        void RenderInventory(Inventory inventory) {
            var text = "";

            foreach (var item in inventory.items) {
                if (item != ResourceType.Empty) {
                    text += ResourceToString(item) + "\n";
                }
            }

            gameRefs.inventoryText.text = text;
        }

        string ResourceToString(ResourceType type) {
            switch (type) {
                case ResourceType.Coin:
                    return "Coin";
                case ResourceType.Herb:
                    return "Herb";
                default:
                    return "";
            }
        }
    }
}