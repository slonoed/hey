using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using Voody.UniLeo;

namespace YANTH {
    sealed class EcsStartup : MonoBehaviour {
        public GameConfigSO gameConfig;
        public Cinemachine.CinemachineVirtualCamera virtualCamera;
        // TODO replace with grouped object to avoid clashes
        public TMP_Text inventoryText;

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
                .ConvertScene()

                .Add(new HeroInitSystem())
                .Add(new PlayerInitSystem())

                .Add(new HeroMovementSystem())
                .Add(new PlayerMovementSystem())
                .Add(new ResourceGenerationSystem())
                .Add(new ResourceGatheringSystem())
                .Add(new InventoryRenderSystem())
                .Add(new DestroySystem())

                // register one-frame components (order is important), for example:
                // .OneFrame<TestComponent1> ()
                // .OneFrame<TestComponent2> ()

                .Inject(gameConfig)
                .Inject(virtualCamera)
                .Inject(inventoryText)
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