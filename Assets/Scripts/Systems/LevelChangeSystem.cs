using System;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Analytics;

namespace YANTH {
    sealed class LevelChangeSystem : IEcsRunSystem {
        readonly GameConfigSO gameConfig = null;
        readonly GameRefs gameRefs = null;
        readonly EcsFilter<Hero, ZoneEnter, Trnsfrm, Health> heroFilter = null;
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
                    ref var heroTransform = ref heroFilter.Get3(hi);
                    ref var health = ref heroFilter.Get4(hi);

                    if (numOfLevels >= hero.level) {
                        gameRefs.uiManager.RunAction("openLevel" + (hero.level - 1) + "End");

                        heroTransform.value.position = gameRefs.levelStartPoints.GetChild(hero.level - 1).position;
                        health.value = Math.Max(health.value, 100 * gameConfig.nextLevelHealthRestoration / health.max);

                        AnalyticsUtils.Emit("level_complete", "number", (hero.level - 1).ToString());

                    } else {
                        AnalyticsUtils.Emit("game_end", "coins", hero.wallet.ToString());
                        gameRefs.uiManager.RunAction("openGameEnd");
                        gameRefs.uiManager.coinsCounterText.text = hero.wallet.ToString();

                        hero.level = 1;
                        hero.wallet = 0;
                        heroTransform.value.position = gameRefs.levelStartPoints.GetChild(hero.level - 1).position;
                        health.value = health.max;
                    }

                    ref var playerTransform = ref hero.player.Get<Trnsfrm>();
                    playerTransform.value.position = heroTransform.value.position + Vector3.up * 3 + Vector3.right * 2;
                }

            }

            // When user clicked button to go to next level
            if (shouldSwitchLevel) {
                gameRefs.uiManager.RunAction("play");
                AnalyticsUtils.Emit("level_start");
            }
        }
    }
}