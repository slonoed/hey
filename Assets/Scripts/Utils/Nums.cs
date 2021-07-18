namespace YANTH {
    public class Nums {
        public static bool IsBetween(float lower, float num, float upper) {
            return num > lower && num < upper;
        }

        public static bool IsOutside(float lower, float num, float upper) {
            return num < lower || num > upper;
        }
    }

}