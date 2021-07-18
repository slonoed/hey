using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YANTH {
    [CreateAssetMenu(fileName = "Game/Config")]
    public class GameConfigSO : ScriptableObject {
        [Header("Prefabs")]
        public GameObject heroPrefab;
        public GameObject playerPrefab;
        public GameObject coinPrefab;

        [Header("Hero")]
        [Range(0.1f, 10f)]
        [Tooltip("How fast hero is moving")]
        public float heroSpeed = 1f;

        [Header("Player")]
        [Range(0.1f, 10f)]
        [Tooltip("How fast player can move it's character")]
        public float playerSpeed = 1f;

        [Header("Resources")]
        [Tooltip("How far away from hero resources are generated. Increase if they appear in camera view")]
        public float resourceGenerationDistance = 10f;
        [Tooltip("How many resources will be generated in sliding window")]
        public float resourceDencity = 4f;
    }
}