using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

public partial class TacticsCameraMovementService : RefCounted
{
    public const int DeltaSmoothing = 8;
    public const int FastSmoothing = 100;
    public const int VelocitySmoothing = 8;
    public const float MinThreshold = 0.1f;
    public const float MinDistance = 0.25f;
    public const int SpeedDivider = 4;

    public TacticsCameraResource resource;
    public TacticsControlsResource controls;

    public TacticsCameraMovementService(
        TacticsCameraResource _resource,
        TacticsControlsResource _controls
    )
    {
        resource = _resource;
        controls = _controls;
    }

    public void MoveCamera(float h, float v, bool joystick, double delta, TacticsCamera camera)
    {
        if (resource.target != null || (h == 0 && v == 0))
            return;

        float angle = (float)Math.Atan2((double)-h, (double)v) + camera.tPivot.Rotation.Y;
        Vector3 dir = Vector3.Forward.Rotated(Vector3.Up, angle);

        float speedMultiplier = joystick ? resource.joyPanSpeed : resource.mousePanSpeed;
        resource.targetVelocity = dir * resource.moveSpeed * speedMultiplier;

        if (joystick)
            resource.targetVelocity *= (float)Math.Sqrt(h * h + v * v);

        camera.Velocity = camera.Velocity.Lerp(
            resource.targetVelocity * VelocitySmoothing,
            resource.smoothing * DeltaSmoothing * (float)delta
        );

        if (camera.Velocity.Length() > MinThreshold)
        {
            Vector3 newPosition = camera.GlobalPosition + camera.Velocity * (float)delta;
            Vector3 distanceFromCenter = newPosition - resource.boundaryCenter;

            if (distanceFromCenter.Length() > resource.boundaryRadius)
            {
                Vector3 clampedPosition =
                    resource.boundaryCenter
                    + distanceFromCenter.Normalized() * resource.boundaryRadius;
                camera.GlobalPosition = clampedPosition;
                camera.Velocity = Vector3.Zero;
            }
            else
                camera.MoveAndSlide();
        }
    }

    public void FocusOnTarget(TacticsCamera camera)
    {
        if (resource.target == null)
            return;

        Vector3 from = camera.GlobalPosition;
        Vector3 to = resource.target.GlobalPosition;
        if (from.DistanceTo(to) <= MinDistance)
        {
            resource.target = null;
            return;
        }

        Vector3 vel = (to - from) * resource.moveSpeed / SpeedDivider;

        Vector3 distanceFromCenter = to - resource.boundaryCenter;
        if (distanceFromCenter.Length() > resource.boundaryRadius)
        {
            to =
                resource.boundaryCenter + distanceFromCenter.Normalized() * resource.boundaryRadius;
            vel = (to - from) * resource.moveSpeed / SpeedDivider;
        }

        camera.Velocity = vel;
        camera.UpDirection = Vector3.Up;
        camera.MoveAndSlide();

        // Don't know what this line does but it was translated over;
        camera.Velocity = camera.Velocity;
    }

    public void StabilizeCamera(double delta, TacticsCamera camera)
    {
        if (resource.target != null)
            return;

        resource.targetVelocity = Vector3.Zero;
        camera.Velocity = camera.Velocity.Lerp(
            Vector3.Zero,
            resource.smoothing * FastSmoothing * (float)delta
        );
        if (camera.Velocity.Length() > MinThreshold)
            camera.MoveAndSlide();
    }
}
