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
    private string lastCollisionReport = string.Empty;

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
        camera.Velocity = new Vector3(camera.Velocity.X, 0.0f, camera.Velocity.Z);
        float maximumSpeed = resource.targetVelocity.Length() * resource.maximumSpeed;
        if (camera.Velocity.Length() > maximumSpeed)
            camera.Velocity = camera.Velocity.Normalized() * maximumSpeed;

        if (camera.Velocity.Length() > MinThreshold)
        {
            Vector3 newPosition = camera.GlobalPosition + camera.Velocity * (float)delta;
            Vector3 distanceFromCenter = newPosition - resource.boundaryCenter;
            distanceFromCenter.Y = 0.0f;

            if (distanceFromCenter.Length() > resource.boundaryRadius)
            {
                Vector3 clampedPosition =
                    resource.boundaryCenter
                    + distanceFromCenter.Normalized() * resource.boundaryRadius;
                clampedPosition.Y = camera.GlobalPosition.Y;
                camera.GlobalPosition = clampedPosition;

                Vector3 boundaryNormal = distanceFromCenter.Normalized();
                float outwardVelocity = camera.Velocity.Dot(boundaryNormal);
                if (outwardVelocity > 0.0f)
                    camera.Velocity -= boundaryNormal * outwardVelocity;
            }
            else
            {
                camera.MoveAndSlide();
                ReportSlideCollisions(camera);
            }
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
        ReportSlideCollisions(camera);

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
        {
            camera.MoveAndSlide();
            ReportSlideCollisions(camera);
        }
    }

    private void ReportSlideCollisions(TacticsCamera camera)
    {
        if (camera.GetSlideCollisionCount() == 0)
        {
            lastCollisionReport = string.Empty;
            return;
        }

        for (int index = 0; index < camera.GetSlideCollisionCount(); index++)
        {
            KinematicCollision3D collision = camera.GetSlideCollision(index);
            GodotObject collider = collision.GetCollider();
            string colliderName = collider is Node colliderNode
                ? colliderNode.GetPath().ToString()
                : collider?.ToString() ?? "<freed object>";
            string collisionLayer = collider is CollisionObject3D collisionObject
                ? collisionObject.CollisionLayer.ToString()
                : "n/a";
            string report =
                $"TacticsCamera collision: collider={colliderName}, "
                + $"type={collider?.GetClass() ?? "<null>"}, "
                + $"layer={collisionLayer}, shape={collision.GetColliderShape()}, "
                + $"normal={collision.GetNormal()}, position={collision.GetPosition()}";

            if (report != lastCollisionReport)
            {
                GD.PushWarning(report);
                lastCollisionReport = report;
            }
        }
    }
}
