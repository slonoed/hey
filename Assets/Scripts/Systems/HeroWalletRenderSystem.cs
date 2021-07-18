using Leopotam.Ecs;

namespace YANTH {
    sealed class HeroWalletRenderSystem : IEcsRunSystem {
        readonly GameRefs gameRefs = null;

        readonly EcsFilter<Hero> heroFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var hi in heroFilter) {
                ref var hero = ref heroFilter.Get1(hi);
                gameRefs.heroCoinsText.text = hero.wallet.ToString();
            }
        }
    }
}