using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YANTH {
    public class ColliderTEst : MonoBehaviour {
        /// <summary>
        /// Sent when another object enters a trigger collider attached to this
        /// object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision.</param>
        void OnTriggerEnter2D(Collider2D other) {
            // Lg.Log("TRIGGER");
        }

        /// <summary>
        /// OnTriggerStay is called once per frame for every Collider other
        /// that is touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerStay2D(Collider2D other) {

            // Lg.Log("STAYS", other.IsTouching(GetComponent<Collider2D>()));
        }
    }
}