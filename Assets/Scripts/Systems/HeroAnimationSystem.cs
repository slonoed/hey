using Leopotam.Ecs;

namespace YANTH {
    sealed class HeroAnimationSystem : IEcsRunSystem {
        readonly EcsFilter<Hero, Anmtr> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var hero = ref heroFilter.Get1(hi);
                ref var animator = ref heroFilter.Get2(hi);
                var stateName = HeroStateToAnimationStateName(hero.state);
                if (stateName != animator.currentState) {
                    animator.value.Play(stateName);
                    animator.currentState = stateName;
                }
            }
        }

        string HeroStateToAnimationStateName(HeroState state) {
            switch (state) {
                case HeroState.Roam:
                    return "Walk";
                case HeroState.Fight:
                    return "Fight";
                case HeroState.Wait:
                    return "Idle";
                case HeroState.Death:
                    return "Death";
                default:
                    return "";
            }
        }
    }
}