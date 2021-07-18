using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YANTH {
    public class UIManager : MonoBehaviour {
        public GameObject mainMenu;
        public GameObject credits;
        public GameObject prelude;
        public GameObject levelEnd;
        public GameObject gameEnd;
        GameObject[] all;

        public bool showMainScreenOnStart;

        public string currentAction;

        void Start() {
            all = new GameObject[] { mainMenu, credits, prelude, levelEnd, gameEnd };
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
                case "openLevelEnd":
                    TogglePanel(levelEnd);
                    return;
                case "openGameEnd":
                    TogglePanel(gameEnd);
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