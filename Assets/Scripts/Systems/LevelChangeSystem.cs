using Leopotam.Ecs;
using UnityEngine;

namespace YANTH {
    sealed class LevelChangeSystem : IEcsRunSystem {
        readonly GameRefs gameRefs = null;
        readonly EcsFilter<Hero, ZoneEnter, Trnsfrm> heroFilter = null;
        readonly EcsFilter<Player, Trnsfrm> playerFilter = null;

        string lastAction;

        void IEcsRunSystem.Run() {
            var numOfLevels = gameRefs.levelStartPoints.childCount;
            var currentAction = gameRefs.uiManager.currentAction;
            var shouldSwitchLevel = currentAction != lastAction && currentAction == "nextLevel";
            lastAction = currentAction;

            foreach (var hi in heroFilter) {
                ref var hero = ref heroFilter.Get1(hi);
                // Check we need to show end level/game screen
                ref var zone = ref heroFilter.Get2(hi);
                if (zone.tags.Contains("levelEnd")) {
                    hero.level++;

                    if (numOfLevels >= hero.level) {
                        gameRefs.uiManager.RunAction("openLevelEnd");
                        ref var heroTransform = ref heroFilter.Get3(hi);
                        heroTransform.value.position = gameRefs.levelStartPoints.GetChild(hero.level - 1).position;
                        ref var playerTransform = ref hero.player.Get<Trnsfrm>();
                        playerTransform.value.position = heroTransform.value.position + Vector3.up * 3 + Vector3.right * 2;
                    } else {
                        gameRefs.uiManager.RunAction("openGameEnd");
                    }
                }

            }

            // When user clicked button to go to next level
            if (shouldSwitchLevel) {
                Lg.Log("SWITCH");
                gameRefs.uiManager.RunAction("play");
            }
        }
    }
}