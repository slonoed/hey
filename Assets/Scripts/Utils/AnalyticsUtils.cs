using System.Collections.Generic;
using UnityEngine.Analytics;

namespace YANTH {
    public class AnalyticsUtils {
        // How to use:
        // AnalyticsUtils.Emit("my_cool_event", "user", "Vasya", "age", "34")
        public static void Emit(string eventName, params string[] data) {
            var eventData = new Dictionary<string, object>();
            for (int i = 0; i < data.Length; i += 2) {
                eventData.Add(data[i], data[i + 1]);
            }

            Analytics.CustomEvent(eventName, eventData);
        }
    }
}