using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts {
    public abstract class BaseSpherePointsEditor : MonoBehaviour {
        public LayerMask ground;

        bool editMode;
        public void SetEditMode(bool value) {
            editMode = value;
        }

        Vector3 activeCursorPoint;
        float activeRadius;
        public void SetCursor(Vector3 cur, float r = 0.1f) {
            activeCursorPoint = cur;
            activeRadius = r;
        }

        public bool IsCursorInsideAnySphere() {
            return SelectedSphere() != -1;
        }

        internal int SelectedSphere() {
            for (int i = 0; i < spheres.Count; i++) {
                if (spheres[i].HasPoint(activeCursorPoint)) { return i; }
            }
            return -1;
        }

        internal bool AffectedByAnySphere(Vector3 p) {
            bool hasAnySourceSphere = false;
            bool insideAnySourceSphere = false;
            for (int i = 0; i < spheres.Count; i++) {
                if (spheres[i].isSource) {
                    hasAnySourceSphere = true;
                    if (spheres[i].HasPoint(p)) {
                        insideAnySourceSphere = true;
                    }
                }
                else
                if (spheres[i].HasPoint(p)) { return true; }
            }
            return hasAnySourceSphere && !insideAnySourceSphere;
        }

        Color cursor = new Color(1, 1, 1, 0.3f);
        Color selectedSphere = new Color(1, 0, 0, 0.7f);
        Color cutSphere = new Color(1, 0.2f, 0, 0.5f);
        Color sourceSphere = new Color(0.2f, 1f, 0, 0.5f);
        Color editSphere = new Color(1, 1, 0, 0.6f);

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos() {
            if (!editMode) return;

            if (!inSizeEdit) {
                RaycastHit hit;
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit, 100, ground)) {
                    SetCursor(hit.point);
                }
            }

            bool locked = false;
            for (int i = 0; i < spheres.Count; i++) {
                if (inSizeEdit && i == indexLocked) { Gizmos.color = editSphere; locked = true; }
                else
                if (spheres[i].HasPoint(activeCursorPoint) && !locked) { Gizmos.color = selectedSphere; locked = true; }
                else Gizmos.color = spheres[i].isSource ? sourceSphere : cutSphere;
                Gizmos.DrawSphere(spheres[i].pos, spheres[i].r);
            }
            if (!locked) {
                Gizmos.color = cursor;
                Gizmos.DrawSphere(activeCursorPoint, 0.1f);
            }
        }
#endif

        public void AddSphere() {
            spheres.Add(new AreaSphere(activeCursorPoint, activeRadius));
        }

        public void ChangeSphereType() {
            spheres[SelectedSphere()].ChangeType();
        }

        public void RemoveSphere() {
            spheres.RemoveAt(SelectedSphere());
        }

        bool inSizeEdit = false;
        int indexLocked = -1;
        public bool LockSphereForEdit(bool value) {
            if (value) indexLocked = SelectedSphere();
            else indexLocked = -1;
            return inSizeEdit = value;
        }

        public void ChangeRadius(Vector2 vector2) {
            var index = indexLocked;
            if (index != -1) {
                var newRadius = spheres[index].r + vector2.x;
                if (newRadius > 0.1f) spheres[index].r = newRadius;
            }
        }

        [HideInInspector]
        public List<AreaSphere> spheres = new List<AreaSphere>();

        [Serializable]
        public class AreaSphere {
            public Vector3 pos;
            public float r;
            public bool isSource = false;
            public AreaSphere(Vector3 position, float radius = 0.1f) {
                pos = position;
                r = radius;
            }

            internal void ChangeType() {
                isSource = !isSource;
            }

            internal bool HasPoint(Vector3 p) {
                return (pos - p).magnitude < r;
            }
        }

        public abstract void RefreshPoints();
    }
}
