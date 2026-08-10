using System;
using System.Linq;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Godot;
using Godot.Collections;

public partial class TacticsCameraPanningService : RefCounted
{
    public TacticsCameraResource resource;

    public TacticsCameraPanningService(TacticsCameraResource _resource)
    {
        resource = _resource;
    }

    public bool IsCursorNearEdge(TacticsCamera camera)
    {
        RefreshCamViewportSize(camera);
        resource.mousePosition = camera.GetViewport().GetMousePosition();
        Dictionary panning = GetMousePanningValues();

        return (float)panning["h"] != 0 || (float)panning["v"] != 0;
    }

    public void WasdPan(double delta, TacticsCamera camera, Vector2 inputDirection)
    {
        DoPan(inputDirection.X, inputDirection.Y, delta, camera);
    }

    public void EdgePan(double delta, TacticsCamera camera)
    {
        RefreshCamViewportSize(camera);
        Dictionary panning = GetMousePanningValues();
        float hVal = (float)panning["h"];
        float vVal = (float)panning["v"];

        DoPan(hVal, vVal, delta, camera);
    }

    public bool DoPan(float h, float v, double delta, TacticsCamera camera)
    {
        if (h != 0 || v != 0)
        {
            resource.panningTimer += (float)delta;
            if (resource.panningTimer >= TacticsCameraResource.PanningDelay)
            {
                camera.MoveCamera(h, v, false, delta);
                return true;
            }
        }
        else
            resource.panningTimer = 0.0f;

        return false;
    }

    public bool RefreshCamViewportSize(TacticsCamera camera)
    {
        Vector2I vpSize = (Vector2I)camera.GetViewport().GetVisibleRect().Size;
        if (vpSize != resource.viewportSize)
        {
            resource.viewportSize = vpSize;
            return true;
        }
        else
            return false;
    }

    public Dictionary GetMousePanningValues()
    {
        float h = 0.0f;
        float v = 0.0f;

        if (resource.mousePosition.X <= resource.borderPanPixelThreshold)
            h = -1.0f;
        else if (
            resource.mousePosition.X
            >= resource.viewportSize.X - resource.borderPanPixelThreshold
        )
            h = 1.0f;

        if (resource.mousePosition.Y <= resource.borderPanPixelThreshold)
            v = 1.0f;
        else if (
            resource.mousePosition.Y
            >= resource.viewportSize.Y - resource.borderPanPixelThreshold
        )
            v = -1.0f;

        return new Dictionary { { "h", h }, { "v", v } };
    }
}
