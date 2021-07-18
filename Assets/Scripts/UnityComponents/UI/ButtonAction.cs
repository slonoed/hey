using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YANTH {
    [RequireComponent(typeof(Button))]
    public class ButtonAction : MonoBehaviour {
        public string actionName;

        UIManager uiManager;
        Button button;

        void Start() {
            if (actionName == "") {
                Lg.Warn("Button action name is empty");
            }

            uiManager = GetComponentInParent<UIManager>();
            if (uiManager == null) {
                Lg.Warn("Button can't find UI Manager on parent");
            }

            button = GetComponent<Button>();

            button.onClick.AddListener(HandleClick);
        }

        void HandleClick() {
            uiManager.RunAction(actionName);
        }
    }

}