using UnityEngine;

namespace Assets.Scripts {
    [ExecuteInEditMode]
    public class DetailsScatter : BaseScatterObject {

        public GameObject[] detailVariants;

        public Vector2 scaleRange = new Vector2(0, 1);
        public Vector2 clumpsScale = new Vector2(0, 1);
        public Vector3 rotationVariants = new Vector3(0, 0, 1);
        
        override public void RefreshPoints() {
            RemoveChildren();
            InitSeeds();
            ScatterDetails(size, transform.position, totalCount);
        }

        private void ScatterDetails(Vector2 size, Vector3 origin, int count) {
            var rayLength = maxWorldHeight - minWorldHeight;
            for (int i = 0; i < count; ++i) {
                Ray ray = GetRandomRayDownWithinArea(size, origin);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, rayLength, ground)) {
                    Vector3 newOrigin = GetLocalHitPosition(origin, hit);

                    var rotationX = mainSeed.Range(-1.0f, 1.0f);
                    var rotationY = mainSeed.Range(-1.0f, 1.0f);
                    var rotationZ = mainSeed.Range(-1.0f, 1.0f);

                    var height = mainSeed.Range(scaleRange.x, scaleRange.y);

                    for (int f = 0; f < clumpsSize + 1; f++) {
                        Vector3 offset = FastMath.Zero;
                        if (f != 0) {
                            if (clumpsLocationSeed.NextDouble() > 0.5f) continue;
                            else {
                                var clumpVariation = (f + 0.1f) / (clumpsSize + 1);
                                rotationZ += clumpVariation;
                                height = clumpsSpreadSeed.Range(clumpsScale.x, clumpsScale.y);

                                if (f > clumpsSameOriginCount) {
                                    var x = GetOffset(clumpsSpreadSeed, scale: clumpsRadius);
                                    var z = GetOffset(clumpsSpreadSeed, scale: clumpsRadius);
                                    offset = new Vector3(x, 0, z);
                                }
                            }
                        }
                        //Instantiate detail at position according to normal
                        var detail = detailVariants[mainSeed.Range(0, detailVariants.Length)];
                        if (!AffectedByAnySphere(hit.point)) {
                            var t = (PrefabUtility.GetPrefabType(detail) == PrefabType.Prefab ?
                                (GameObject)PrefabUtility.InstantiatePrefab(detail) :
                                Instantiate(detail)).transform;
                            t.position = origin + newOrigin + offset;
                            t.rotation = detail.transform.rotation * Quaternion.FromToRotation(FastMath.Up, hit.normal)
                                * Quaternion.Euler(
                                    rotationVariants.x * rotationX * 180,
                                    rotationVariants.y * rotationY * 180,
                                    rotationVariants.z * rotationZ * 180);
                            t.parent = transform;
                            t.localScale *= height;
                        }
                    };
                }
            }
        }
    }
}
