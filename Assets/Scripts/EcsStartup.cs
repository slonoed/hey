using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using Voody.UniLeo;

namespace YANTH {
    [System.Serializable]
    public class GameRefs {
        public TMP_Text heroCoinsText;
        public GameObject inventoryPanel;
        public Cinemachine.CinemachineVirtualCamera virtualCamera;
        public Camera camera;
        public Transform levelStartPoints;
        public UIManager uiManager;
    }

    sealed class EcsStartup : MonoBehaviour {
        public static EcsWorld globalWorld;

        public GameConfigSO gameConfig;

        public GameRefs gameRefs;

        EcsWorld world;
        EcsSystems systems;

        void Start() {
            world = new EcsWorld();
            globalWorld = world;

            systems = new EcsSystems(world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
#endif
            systems
                .ConvertScene()

                .Add(new HeroInitSystem())
                .Add(new PlayerInitSystem())

                .Add(new SpeechZoneCreationSystem())
                .Add(new ZoneCreationSystem())
                .Add(new EnemyGenerationSystem())
                .Add(new SpeechZoneCreationSystem())
                .Add(new HeroMovementSystem())
                .Add(new FightSystem())
                .Add(new EnemyMoveSystem())
                .Add(new PlayerMovementSystem())
                .Add(new HeroAnimationSystem())
                .Add(new EnemyAnimationSystem())
                .Add(new ResourceGenerationSystem())
                .Add(new ResourceGatheringSystem())
                .Add(new InventoryRenderSystem())
                .Add(new HealthbarSystem())
                .Add(new InventoryDropSystem())
                .Add(new InventoryDropTransferSystem())
                .Add(new HeroDropReceivingSystem())
                .Add(new ZoneSystem())
                .Add(new HeroWalletRenderSystem())
                .Add(new HeroDropReceivingTweenSystem())
                .Add(new SoundSystem())
                .Add(new HeroSpeechZoneSystem())
                .Add(new SpeechBubbleSystem())
                .Add(new LevelChangeSystem())
                .Add(new DestroySystem())

                // register one-frame components (order is important), for example:
                // .OneFrame<TestComponent1> ()
                // .OneFrame<TestComponent2> ()

                .Inject(gameConfig)
                .Inject(gameRefs)
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