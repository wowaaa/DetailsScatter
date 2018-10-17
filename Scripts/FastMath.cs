using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts {
    public static class FastMath {
        public static System.Random rn = new System.Random();

        public static Vector3 Up = Vector3.up;
        public static Vector3 Down = Vector3.down;
        public static Vector3 Forward = Vector3.forward;
        public static Vector3 Right = Vector3.right;

        public static Vector3 One = Vector3.one;

        public static Vector3 Zero = Vector3.zero;
        public static Vector2 Zero2 = Vector2.zero;
        public static Vector4 Zero4 = Vector4.zero;

        public static Vector2 Right2 =new Vector2(1, 0);
        public static Vector2 Up2 = new Vector2(0, 1);
        public static Vector2 One2 = new Vector2(1, 1);

        public static Vector3 Flat = new Vector3(1, 0, 1);

        public static Stopwatch debugTimer = new Stopwatch();

        public static T GetRandom<T>(this T[] elements) {
            return elements[rn.Next(elements.Length)];
        }

        public static T GetRandom<T>(this T[] elements, out int index) {
            return elements[index = rn.Next(elements.Length)];
        }

        public static float Range(this System.Random rn,float min,float max) {
            return min + (float)rn.NextDouble() * (max - min);
        }

        public static int Range(this System.Random rn, int min, int max) {
            return rn.Next(min, max);
        }
    }

    public static class Strings {
        public static string Empty = "";
    }
}
