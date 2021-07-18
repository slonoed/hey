using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class PlayerMovementSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly GameRefs gameRefs = null;

        readonly EcsFilter<Player, Trnsfrm> playerFilter = null;
        readonly EcsFilter<Hero, Trnsfrm, ZoneTarget> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in playerFilter) {
                if (!gameConfig.playerCanMoveWhenDrop && playerFilter.GetEntity(pi).Has<DropProducer>()) {
                    continue;
                }

                ref var transform = ref playerFilter.Get2(pi);

                // if (transform.value.position.y < gameRefs.playerStartPoint.position.y)
                //     continue;

                transform.value.position += CurrentDirection() * Time.deltaTime * gameConfig.playerSpeed;

                foreach (var hi in heroFilter) {
                    ref var zone = ref heroFilter.Get3(hi);
                    if (zone.tags.Contains("noCameraFollow")) {
                        // TODO: stop camera inertia
                        gameRefs.virtualCamera.Follow = null;
                    }
                    else
                    {
                        // TODO: resume hero following
                        // gameRefs.virtualCamera.Follow = ;
                    }
                }

                // Keep player in camera view
                Vector3 pos = gameRefs.camera.WorldToViewportPoint(transform.value.position);
                pos.x = Mathf.Clamp(pos.x, 0.05f, 0.95f);
                pos.y = Mathf.Clamp(pos.y, 0.07f, 0.93f);
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