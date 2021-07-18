using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YANTH {
    [CreateAssetMenu(fileName = "Game/Config")]
    public class GameConfigSO : ScriptableObject {
        public GameObject heroPrefab;
        public GameObject playerPrefab;

        [Range(0.1f, 10f)]
        [Tooltip("How fast player can move it's character")]
        public float playerSpeed = 1f;
    }
}