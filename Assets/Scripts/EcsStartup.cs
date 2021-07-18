using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class EcsStartup : MonoBehaviour {
        public GameConfigSO gameConfig;
        public Cinemachine.CinemachineVirtualCamera virtualCamera;

        EcsWorld world;
        EcsSystems systems;

        void Start() {
            world = new EcsWorld();
            systems = new EcsSystems(world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
#endif
            systems
                .Add(new HeroInitSystem())
                .Add(new PlayerInitSystem())

                .Add(new HeroMovementSystem())
                .Add(new PlayerMovementSystem())
                .Add(new ResourceGenerationSystem())
                .Add(new ResourceGatheringSystem())

                // register one-frame components (order is important), for example:
                // .OneFrame<TestComponent1> ()
                // .OneFrame<TestComponent2> ()

                .Inject(gameConfig)
                .Inject(virtualCamera)
                .Init();
        }

        void Update() {
            systems?.Run();
        }

        void OnDestroy() {
            if (systems != null) {
                systems.Destroy();
                systems = null;
                world.Destroy();
                world = null;
            }
        }
    }
}