using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace YANTH {
    public class UIManager : MonoBehaviour {
        public GameObject mainMenu;
        public GameObject credits;
        public GameObject prelude;
        public GameObject level1End;
        public GameObject level2End;
        public GameObject gameEnd;
        public GameObject death;
        public GameObject blackScreen;
        GameObject[] all;

        public bool showMainScreenOnStart;
        public string currentAction;

        public TMPro.TMP_Text coinsCounterText;

        CanvasGroup blackCanvas;

        void Start() {
            blackCanvas = blackScreen.GetComponent<CanvasGroup>();
            
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
                    TogglePanel(prelude, 1f);
                    return;
                case "openLevel1End":
                    TogglePanel(level1End, 1f);
                    return;
                case "openLevel2End":
                    TogglePanel(level2End, 1f);
                    return;
                case "openGameEnd":
                    TogglePanel(gameEnd, 1f);
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

        void TogglePanel(GameObject panel, float fadeTime = 0.15f) {
            blackScreen.SetActive(true);
            Time.timeScale = 0f;
            blackCanvas.DOFade(1, fadeTime).SetUpdate(true).OnComplete(() => {
                ToggleAllOff();
                panel.SetActive(true);
                blackCanvas.DOFade(0, fadeTime).SetUpdate(true).OnComplete(() => {
                    blackScreen.SetActive(false);
                });
            });
        }

        void ToggleAllOff() {
            foreach (var panel in all) {
                panel.SetActive(false);
            }
            Time.timeScale = 1f;
        }
    }
}