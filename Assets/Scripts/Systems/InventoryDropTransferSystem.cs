using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    // Transferts items from producer with invetory to receiver
    sealed class InventoryDropTransferSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly EcsWorld world = null;
        readonly EcsFilter<DropProducer, Inventory, Trnsfrm, Player> producerFilter = null;
        // readonly EcsFilter<Player> playerFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in producerFilter) {
                ref var producer = ref producerFilter.Get1(pi);
                if (producer.lastDropTime > Time.time - 0.3f) {
                    continue;
                }
                ref var inventory = ref producerFilter.Get2(pi);
                ref var transform = ref producerFilter.Get3(pi);
                ref var playerEntity = ref producerFilter.GetEntity(pi);
                
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

                        SoundUtils.Create(world, gameConfig.heroInventorySound, transform.value.position);
                        SpeechUtils.Add(playerEntity, new []{"Take this!","Eat this!","I'm helping!","Grab a bite!","Dig in!"}, chance: 0.3f, TTL: 0.7f);

                        // STOP AFTER ONE ITEM TRANSFERED
                        break;
                    }
                }
            }
        }
    }
}