using UnityEngine;

namespace Script.Extension.Unity {
    public static class Vector2Extension {
        public static Vector2 WithX(this Vector2 vector, float x) {
            return new Vector2(x, vector.y);
        }

        public static Vector2 WithY(this Vector2 vector, float y) {
            return new Vector2(vector.x, y);
        }
    }

    public class test {
    
    }
}