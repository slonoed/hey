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
        public GameObject herbPrefab;
        public GameObject healthbarPrefab;
        [Tooltip("Used to show speech bubble near character")]
        public GameObject speechBubblePrefab;

        [Header("Hero")]
        [Range(0.1f, 10f)]
        [Tooltip("How fast hero is moving")]
        public float heroSpeed = 1f;
        public int heroMaxHP = 100;
        public float heroAttackDelay = 1f;
        public int attack = 10;
        public AudioClip heroFightSound;
        public AudioClip heroInventorySound;

        [Header("Player")]
        [Range(0.1f, 10f)]
        [Tooltip("How fast player can move it's character")]
        public float playerSpeed = 1f;
        public int playerInventorySize = 10;

        [Header("Resources")]
        [Tooltip("How far away from hero resources are generated. Increase if they appear in camera view")]
        public float resourceGenerationDistance = 10f;
        [Tooltip("How many resources will be generated in sliding window")]
        public float resourceDencity = 4f;
        [Tooltip("How many HP will be restored when one herb dropped to hero")]
        public int herbHealingFactor = 2;
        public AudioClip coinCollectionSound;
        public AudioClip herbCollectionSound;

        [Header("Enemies")]
        [Tooltip("How many HP enemy will have if not setup in editor")]
        public int enemyMaxHPDefault = 20;
        [Tooltip("How fast enemy will be if not setup in editor")]
        public float enemySpeedDefault = 1f;
        [Tooltip("From what distance enemies see hero and start moving to him?")]
        public float roamDistance = 10f;
        [Tooltip("What damage enemy makes on player")]
        public int enemyAttackDefault = 5;
        [Tooltip("Delay in seconds between hits")]
        public float enemyAttachDelayDefault = 2f;
        public AudioClip enemyFightSound;
        public AudioClip enemyDeathSound;
    }
}