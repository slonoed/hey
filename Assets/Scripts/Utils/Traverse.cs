using UnityEngine;

namespace YANTH {
    public class Traverse {
        public static GameObject FindChildWithTag(GameObject parent, string tag) {
            foreach (Transform t in parent.transform) {
                if (t.tag == tag) {
                    return t.gameObject;
                }

                var child = FindChildWithTag(t.gameObject, tag);
                if (child != null) {
                    return child;
                }
            }

            return null;
        }

        public static GameObject FindChildWithName(GameObject parent, string name) {
            foreach (Transform t in parent.transform) {
                if (t.name == name) {
                    return t.gameObject;
                }

                var child = FindChildWithName(t.gameObject, name);
                if (child != null) {
                    return child;
                }
            }

            return null;
        }
    }
}