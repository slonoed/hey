namespace YANTH {
    public class Lg : System.Object {
        public static void Log(params object[] v) {
#if DEBUG
            UnityEngine.Debug.Log(PrepareOutput(v));
#endif
        }

        public static void Warn(params object[] v) {
            UnityEngine.Debug.LogWarning(PrepareOutput(v));
        }

        static string PrepareOutput(object[] v) {
            string o = "";
            for (int i = 0; i < v.Length; i++) {
                o += " ";
                o += v[i] == null ? "null" : v[i].ToString();
            }

            return o;
        }
    }
}