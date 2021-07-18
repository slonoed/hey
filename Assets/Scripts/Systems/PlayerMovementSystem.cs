using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class PlayerMovementSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Player, Trnsfrm> playerFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                ref var transform = ref playerFilter.Get2(pi);
                transform.value.position += CurrentDirection() * Time.deltaTime * gameConfig.playerSpeed;
            }
        }

        Vector3 CurrentDirection() {
            var dir = new Vector3();

            if (Input.GetKey(KeyCode.W)) {
                dir += Vector3.up;
            }
            if (Input.GetKey(KeyCode.A)) {
                dir += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D)) {
                dir += Vector3.right;
            }
            if (Input.GetKey(KeyCode.S)) {
                dir += Vector3.down;
            }

            return dir;
        }
    }
}