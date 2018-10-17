using Assets.Scripts;
using UnityEditor;
using UnityEngine;

static class Styles {
    private static GUIStyle ToggleButtonStyleNormal = null;
    private static GUIStyle ToggleButtonStyleToggled = null;

    public static GUIStyle GetToggleButtonStyleByState(bool toggled) {
        if (ToggleButtonStyleNormal == null) {
            ToggleButtonStyleNormal = "Button";
            ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
            ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
        }

        return toggled ? ToggleButtonStyleToggled : ToggleButtonStyleNormal;
    }
}

public class BaseSphereEditorInspector : Editor {
    const string helpLabel = "Toggle 'Edit' button to enter 'Edit mode'.\n" +
            "Add spheres by click.\n" +
            "Click on sphere to lock-on for size adjustments.\n" +
            "Delete with pressed Shift click.\n" +
            "Change type from 'Cut' to 'Source' by pressed Ctrl click.";

    bool inEditMode;
    BaseSpherePointsEditor surface;
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (surface == null) {
            surface = (BaseSpherePointsEditor)target;
            surface.SetEditMode(inEditMode = false);
        }
        if (GUILayout.Button("Refresh Points")) {
            surface.RefreshPoints();
        }

        if (GUILayout.Button("Edit", Styles.GetToggleButtonStyleByState(inEditMode))) {
            surface.SetEditMode(inEditMode = !inEditMode);
            SetChildrenRenderState(inEditMode ? EditorSelectedRenderState.Hidden : EditorSelectedRenderState.Highlight);
        }

        GUILayout.Label(helpLabel);

        if (GUILayout.Button("Clear all spheres")) {
            surface.spheres.Clear();
        }
    }

    void SetChildrenRenderState(EditorSelectedRenderState state) {
        var children = surface.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < children.Length; i++) {
            EditorUtility.SetSelectedRenderState(children[i], state);
        }
        SceneView.RepaintAll();
    }

    private void OnDestroy() {
        if (surface != null) surface.SetEditMode(inEditMode = false);
    }

    bool blockingMouseInput;
    bool editSingleSphere = false;
    Vector2 lastMousePos;

    const float relativeSizeChange = 0.01f;

    protected virtual void OnSceneGUI() {
        Event e = Event.current;
        if (!inEditMode || e.button != 0) return;
        if (surface == null) surface = (BaseSpherePointsEditor)target;

        if(e.isMouse){
            if (e.type == EventType.MouseDown) {
                if (inEditMode) {
                    blockingMouseInput = true;
                    ClickOnSurface(e);
                }
            }
            else if (e.type == EventType.MouseDrag) {
                //TODO: drag spheres
            }
            else if (e.type == EventType.MouseMove) {
                if (editSingleSphere) {
                    surface.ChangeRadius((e.mousePosition - lastMousePos) * relativeSizeChange);
                }
                lastMousePos = e.mousePosition;
            }
            else if (e.type == EventType.MouseUp) {
                if (blockingMouseInput) {
                    e.Use();
                }
                blockingMouseInput = false;
            }
        }
        else if (e.type == EventType.Layout && inEditMode) {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
        }

        SceneView.RepaintAll();
    }

    private void ClickOnSurface(Event e) {
        if (editSingleSphere) {
            editSingleSphere = surface.LockSphereForEdit(false);
        }
        else
        if (!surface.IsCursorInsideAnySphere()) {
            surface.AddSphere();
        }
        else
        if (e.shift) {
            surface.RemoveSphere();
        }
        else
        if (e.control) {
            surface.ChangeSphereType();
        }
        else {
            editSingleSphere = surface.LockSphereForEdit(true);
        }
    }
}
