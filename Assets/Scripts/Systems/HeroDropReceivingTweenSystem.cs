using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    // This system adds visual tween when hero receiving items from player
    //
    // TODO slonoed: this system is not very optimized because it creates tween on every run
    // because tweens here are idempotent (scale to fixed value) it works fine
    // Ideally, should only run on change which should require another compoenent
    sealed class HeroDropReceivingTweenSystem : IEcsRunSystem {
        readonly EcsFilter<Hero, Trnsfrm, DropReceiver> receiverFilter = null;
        readonly EcsFilter<Hero, Trnsfrm>.Exclude<DropReceiver> noReceiveFilter = null;

        void IEcsRunSystem.Run() {
            foreach (var ri in receiverFilter) {
                ref var heroTransform = ref receiverFilter.Get2(ri);
                if (!DOTween.IsTweening(heroTransform.value)) {
                    heroTransform.value.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.3f);
                }
            }

            foreach (var nri in noReceiveFilter) {
                ref var heroTransform = ref receiverFilter.Get2(nri);
                if (!DOTween.IsTweening(heroTransform.value)) {
                    heroTransform.value.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
                }
            }
        }
    }
}