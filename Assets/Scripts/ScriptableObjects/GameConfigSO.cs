using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YANTH {
    [CreateAssetMenu(fileName = "Game/Config")]
    public class GameConfigSO : ScriptableObject {
        public GameObject heroPrefab;
    }
}