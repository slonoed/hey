using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YANTH {
    public class UIManager : MonoBehaviour {
        public GameObject mainMenu;
        public GameObject credits;
        public GameObject prelude;
        public GameObject nextLevel;
        GameObject[] all;

        public string lastAction;

        void Start() {
            all = new GameObject[] { mainMenu, credits, prelude, nextLevel };
            foreach (var panel in all) {
                if (panel == null) {
                    Lg.Warn("Panel is not assigned in UIManager");
                }
            }

            if (mainMenu.activeSelf || credits.activeSelf || prelude.activeSelf) {
                Time.timeScale = 0f;
            }
        }

        void LateUpdate() {
            lastAction = "";
        }

        public void RunAction(string action) {
            lastAction = action;

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
                case "play":
                    ToggleAllOff();
                    return;

                default:
                    Lg.Warn("Action is not handled by UIManager", action);
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