using System.Collections.Generic;

namespace YANTH {
    public class SetUtils {
        public static(HashSet<T>, HashSet<T>) Difference<T>(HashSet<T> prev, HashSet<T> next) {
            var added = new HashSet<T>();
            var removed = new HashSet<T>();

            foreach (var item in prev) {
                if (!next.Contains(item)) {
                    removed.Add(item);
                }
            }
            foreach (var item in next) {
                if (!prev.Contains(item)) {
                    added.Add(item);
                }

            }

            return (added, removed);
        }
    }
}