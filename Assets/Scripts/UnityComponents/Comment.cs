using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YANTH {
    // This componen only exists to provide comments in editor
    public class Comment : MonoBehaviour {
        [TextArea(10, 30)]
        public string comment;
    }
}