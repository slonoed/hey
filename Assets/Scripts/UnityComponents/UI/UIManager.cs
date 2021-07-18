using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YANTH {
    public class UIManager : MonoBehaviour {
        public GameObject mainMenu;
        public GameObject credits;
        public GameObject prelude;
        public GameObject level1End;
        public GameObject level2End;
        public GameObject gameEnd;
        public GameObject death;
        GameObject[] all;

        public bool showMainScreenOnStart;
        public string currentAction;

        public TMPro.TMP_Text coinsCounterText;

        void Start() {
            all = new GameObject[] { mainMenu, credits, prelude, level1End, level2End, gameEnd, death };
            foreach (var panel in all) {
                if (panel == null) {
                    Lg.Warn("Panel is not assigned in UIManager");
                }
            }

            if (mainMenu.activeSelf || credits.activeSelf || prelude.activeSelf) {
                Time.timeScale = 0f;
            }

            if (showMainScreenOnStart) {
                RunAction("openMainMenu");
            }
        }

        public void RunAction(string action) {
            AnalyticsUtils.Emit("ui_action", "action", action);
            currentAction = action;
            switch (action) {
                case "openMainMenu":
                    TogglePanel(mainMenu);
                    return;
                case "openCredits":
                    TogglePanel(credits);
                    return;
                case "openPrelude":
                    TogglePanel(prelude);
                    return;
                case "openLevel1End":
                    TogglePanel(level1End);
                    return;
                case "openLevel2End":
                    TogglePanel(level2End);
                    return;
                case "openGameEnd":
                    TogglePanel(gameEnd);
                    return;
                case "openDeath":
                    TogglePanel(death);
                    return;
                case "restartGame":
                    Application.LoadLevel(Application.loadedLevel);
                    return;
                case "play":
                    ToggleAllOff();
                    return;

                default:
                    Lg.Warn("Action is not handled by UIManager:", action);
                    return;
            }
        }

        void TogglePanel(GameObject panel) {
            ToggleAllOff();
            panel.SetActive(true);
            Time.timeScale = 0f;
        }

        void ToggleAllOff() {
            foreach (var panel in all) {
                panel.SetActive(false);
            }
            Time.timeScale = 1f;
        }
    }
}