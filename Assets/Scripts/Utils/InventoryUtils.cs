namespace YANTH {
    public class InventoryUtils {
        public static bool HasSpace(Inventory inventory) {
            foreach (var item in inventory.items) {
                if (item == ResourceType.Empty) {
                    return true;
                }
            }
            return false;
        }
        public static bool IsEmpty(Inventory inventory) {
            foreach (var item in inventory.items) {
                if (item != ResourceType.Empty) {
                    return false;
                }
            }
            return true;
        }
    }
}