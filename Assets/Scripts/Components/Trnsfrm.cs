using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    struct Trnsfrm : IEcsAutoReset<Trnsfrm> {
        public Transform value;

        public void AutoReset(ref Trnsfrm c) {
            if (c.value != null) {
                GameObject.Destroy(c.value.gameObject);
                c.value = null;
            }
        }
    }
}