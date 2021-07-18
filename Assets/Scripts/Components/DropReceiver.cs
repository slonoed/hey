using System.Collections.Generic;
using Leopotam.Ecs;

namespace YANTH {
    // Entity is receivin drop right now
    public struct DropReceiver : IEcsAutoReset<DropReceiver> {
        public float lastReceiveTime;
        public Queue<ResourceType> incomming;

        public void AutoReset(ref DropReceiver r) {
            r.incomming = new Queue<ResourceType>();
        }
    }
}