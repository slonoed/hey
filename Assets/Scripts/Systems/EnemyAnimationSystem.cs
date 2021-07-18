using Leopotam.Ecs;

namespace YANTH {
    sealed class EnemyAnimationSystem : IEcsRunSystem {
        readonly EcsFilter<Enemy, Anmtr> enemyFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var ei in enemyFilter) {
                ref var enemy = ref enemyFilter.Get1(ei);
                ref var animator = ref enemyFilter.Get2(ei);
                var stateName = EnemyStateToAnimationStateName(enemy.state);
                if (stateName != animator.currentState) {
                    animator.value.Play(stateName);
                    animator.currentState = stateName;
                }
            }
        }

        string EnemyStateToAnimationStateName(EnemyState state) {
            switch (state) {
                case EnemyState.Roam:
                    return "Idle";
                case EnemyState.Fight:
                    return "Fight";
                case EnemyState.Wait:
                    return "Idle";
                case EnemyState.Death:
                    return "Death";
                default:
                    return "";
            }
        }
    }
}