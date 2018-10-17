# DetailsScatter
Basic details scatter for Unity3D with simple Editor extensions for scatter area handling.

### Provides control over details population:
* Seed: basic random seed
* Clumps Size: Every detail spread point will produce clumps area around itself and this value is the count of additional details inside this area
* Clumps Radius: Size of the clumps area
* Clumps Same Origin Count: Count of additional details (within total Clumps Size) that would share same spread point as the area origin (For example, several grass blades should share its root and this value is the maximum details count without random spread on the clumps area).
* Lock Clumps To Seed: locks clumps random seed generation to the basic seed. Without this toggle, clumps would have total random seed on every refresh.
* Size: Total area size to spread details. Would be shown as gray rectangular gizmo in Editor.
* Total Count: Total details count in this area.
* Ground Offset: Vertical offset for each detail.
* Max World Height: Maximum height to spawn details.
* Min World Height: Minimum height to spawn details.
* Detail Variants: Link prefabs of the instantiated detail variants.
* Scale Range: Uniform scale range of every detail - applied over the prefab values.
* Clumps Scale: Uniform scale range for clumped details - applied over the prefab values.
* Rotation Variants: Rotations variation on each axis. By default, each detail is spawned aligned to surface normal and then affected by this variants.
* Refresh points button: removed all the children of current transform and regenerates all the details according to current settings.
### Edit mode button has self-explanatory instructions.
Just some additional information:
* The spheres right now spawn in global coordinates and won't be affected by object translation.
* The spheres are applied over the basic details generation therefore if you have no details inside the sphere - then none of them would be affected.
* Any *Source* sphere removes all the details outside of it.
