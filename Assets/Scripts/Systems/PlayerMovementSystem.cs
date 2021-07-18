using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class PlayerMovementSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly GameRefs gameRefs = null;

        readonly EcsFilter<Player, Trnsfrm> playerFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                if (!gameConfig.playerCanMoveWhenDrop && playerFilter.GetEntity(pi).Has<DropProducer>()) {
                    continue;
                }

                ref var transform = ref playerFilter.Get2(pi);
                
                // if (transform.value.position.y < gameRefs.playerStartPoint.position.y)
                //     continue;
                
                transform.value.position += CurrentDirection() * Time.deltaTime * gameConfig.playerSpeed;

                // Keep in camera view
                Vector3 pos = gameRefs.camera.WorldToViewportPoint(transform.value.position);
                pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
                pos.y = Mathf.Clamp(pos.y, 0.1f, 0.9f);
                transform.value.position = gameRefs.camera.ViewportToWorldPoint(pos);
            }
        }

        Vector3 CurrentDirection() {
            var dir = new Vector3();

            // GetAxis works for keyboards and gamepads simultaneously
            dir += Vector3.right * Input.GetAxis("Horizontal");
            dir += Vector3.up * Input.GetAxis("Vertical");

            return dir;
        }
    }
}