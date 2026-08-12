using System;
using Game.Models.Config.TacticsConfig;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Godot;

public partial class TacticsCameraZoomService : RefCounted
{
    public const int DeltaSmoothing = 10;

    public TacticsCameraResource resource;

    public TacticsCameraZoomService(TacticsCameraResource _resource)
    {
        resource = _resource;
    }

    public void ZoomCamera(float zoomIncrement)
    {
        resource.targetFOV = Math.Clamp(
            resource.targetFOV + zoomIncrement,
            resource.minZoom,
            resource.maxZoom
        );
    }

    public void ApplyZoomSmoothing(TacticsCamera camera, double delta)
    {
        if (resource.currentFOV != resource.targetFOV)
        {
            resource.currentFOV = Mathf.Lerp(
                resource.currentFOV,
                resource.targetFOV,
                resource.zoomSmoothness * DeltaSmoothing * (float)delta
            );
            camera.camNode.Fov = resource.currentFOV;
        }
    }

    public void ResetCamZoom(Camera3D camNode, TacticsCamera camera)
    {
        resource.targetFOV = TacticsConfig.view["defaultTCamZoom"];

        Tween tween = camera.CreateTween();
        tween
            .TweenProperty(camNode, "Fov", resource.targetFOV, resource.zoomDuration)
            .SetTrans(Tween.TransitionType.Sine);
    }
}
