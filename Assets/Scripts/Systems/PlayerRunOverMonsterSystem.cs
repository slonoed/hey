using Leopotam.Ecs;

namespace YANTH {
    sealed class PlayerRunOverMonsterSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly EcsFilter<Player, Clrd> palyerFilter = null;
        readonly EcsFilter<Enemy, Clrd> enemyFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var pi in palyerFilter) {
                ref var collider = ref palyerFilter.Get2(pi);
                foreach (var ei in enemyFilter) {
                    ref var enemyCollider = ref enemyFilter.Get2(ei);
                    if (enemyCollider.value.IsTouching(collider.value)) {
                        ref var entity = ref palyerFilter.GetEntity(pi);
                        SpeechUtils.Add(entity, gameConfig.sayPlayerOverEnemy, chance : 1f, TTL : 0.5f, false);
                    }
                }
            }
        }
    }
}