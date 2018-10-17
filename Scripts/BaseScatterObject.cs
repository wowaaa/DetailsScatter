using UnityEditor;
using UnityEngine;

namespace Assets.Scripts {
    public class BaseScatterObject : BaseSpherePointsEditor {
        public int seed;
        public int clumpsSize = 0;
        public float clumpsRadius = 0.125f;
        public int clumpsSameOriginCount = 0;
        public bool lockClumpsToSeed = true;
        public Vector2 size;
        public int totalCount;
        public float groundOffset = 0.0f;
        public float maxWorldHeight = 1000;
        public float minWorldHeight = -2;

        protected System.Random mainSeed = new System.Random();
        protected System.Random clumpsLocationSeed = new System.Random();
        protected System.Random clumpsSpreadSeed = new System.Random();

        protected void InitSeeds() {
            mainSeed = new System.Random(seed);
            if (lockClumpsToSeed) {
                clumpsLocationSeed = new System.Random(seed);
                clumpsSpreadSeed = new System.Random(seed);
            }
        }

        protected void RemoveChildren() {
            int childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--) {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        protected Ray GetRandomRayDownWithinArea(Vector2 size, Vector3 origin) {
            var pos = origin;
            pos.y = maxWorldHeight;
            pos.x += size.x * mainSeed.Range(-0.5f, 0.5f) * transform.lossyScale.x;
            pos.z += size.y * mainSeed.Range(-0.5f, 0.5f) * transform.lossyScale.z;

            Ray ray = new Ray(pos, FastMath.Down);
            return ray;
        }

        protected Vector3 GetLocalHitPosition(Vector3 origin, RaycastHit hit) {
            var newOrigin = hit.point;
            newOrigin.y += groundOffset;
            newOrigin.x -= origin.x;
            newOrigin.z -= origin.z;

            newOrigin.x /= transform.lossyScale.x;
            newOrigin.z /= transform.lossyScale.z;
            return newOrigin;
        }

        protected float GetOffset(System.Random rn, float minValue = 0.1f, float scale = 1) {
            var a = (rn.Next(0, 100) - 50) / 50f;
            if (a > 0)
                a = Mathf.Clamp(a, minValue, 1);
            else
                a = Mathf.Clamp(a, -1, -minValue);

            return a * scale;
        }

#if UNITY_EDITOR
        override protected void OnDrawGizmos() {
            if (Selection.Contains(gameObject)) {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 0.5f, size.y));
                base.OnDrawGizmos();
            }
        }
#endif
    }
}
