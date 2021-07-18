using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    // Transferts items from producer with invetory to receiver
    sealed class InventoryDropTransferSystem : IEcsRunSystem {
        readonly EcsFilter<DropProducer, Inventory> producerFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in producerFilter) {
                ref var producer = ref producerFilter.Get1(pi);
                if (producer.lastDropTime > Time.time - 0.3f) {
                    continue;
                }
                ref var inventory = ref producerFilter.Get2(pi);

                var receiverEntity = producer.receiver;
                if (!receiverEntity.IsAlive() || !receiverEntity.Has<DropReceiver>()) {
                    continue;
                }

                ref var receiver = ref receiverEntity.Get<DropReceiver>();
                for (int i = inventory.items.Length - 1; i >= 0; i--) {
                    if (inventory.items[i] != ResourceType.Empty) {
                        receiver.incomming.Enqueue(inventory.items[i]);
                        inventory.items[i] = ResourceType.Empty;
                        producer.lastDropTime = Time.time;

                        // STOP AFTER ONE ITEM TRANSFERED
                        break;
                    }
                }
            }
        }
    }
}