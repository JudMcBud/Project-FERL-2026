using System;
using Godot;
using Godot.Collections;

public partial class MouseClickCapture : InputCapture
{
    #region Methods
    public new Object ProjectMousePosition(int collisionMask, bool isJoystick)
    {
        int RayLength = 1000000;
        Camera3D camera = GetViewport().GetCamera3D();
        Vector2 mousePointerOrigin = !isJoystick
            ? GetViewport().GetMousePosition()
            : GetViewport().GetVisibleRect().Size / 2;

        Vector3 from = camera.ProjectRayOrigin(mousePointerOrigin);
        Vector3 to = from + camera.ProjectRayNormal(mousePointerOrigin) * RayLength;

        PhysicsRayQueryParameters3D rayQuery = PhysicsRayQueryParameters3D.Create(
            from,
            to,
            (uint)collisionMask,
            []
        );
        Dictionary result = GetWorld3D().DirectSpaceState.IntersectRay(rayQuery);
        if (result == null || result.Count == 0 || !result.ContainsKey("collider"))
            return null;
        return (CollisionObject3D)result["collider"];
    }
    #endregion
}
