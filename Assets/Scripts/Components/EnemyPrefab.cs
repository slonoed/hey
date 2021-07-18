using System;
using UnityEngine;

namespace YANTH {
    [Serializable]
    public struct EnemyPrefab {
        public Transform transform;
        public int maxHP;
        public float speed;
    }
}