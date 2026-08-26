using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

public partial class TacticsCameraService : RefCounted
{
    public TacticsCameraResource resource;
    public TacticsControlsResource controls;
    public TacticsCameraMovementService move;
    public TacticsCameraZoomService zoom;
    public TacticsCameraRotationService rotate;
    public TacticsCameraPanningService pan;
    private TacticsCamera camera;
    private Camera3D camNode;

    public const int DeltaSmoothing = 10;
    public const float MinVel = 0.01f;

    public TacticsCameraService(TacticsCameraResource _resource, TacticsControlsResource _controls)
    {
        resource = _resource;
        controls = _controls;
        move = new TacticsCameraMovementService(resource, controls);
        zoom = new TacticsCameraZoomService(resource);
        rotate = new TacticsCameraRotationService(resource, controls);
        pan = new TacticsCameraPanningService(resource);
    }

    public void Setup(TacticsCamera camera, Camera3D camNode)
    {
        this.camera = camera;
        this.camNode = camNode;
        if (controls == null)
        {
            GD.PushError(
                "TacticsControls needs a ControlResource from /models/view/control/tactics/"
            );
        }
        if (resource == null)
        {
            GD.PushError("TacticsCamera needs a CameraResource from /models/view/camera/tactics/");
        }
        else
        {
            resource.targetFOV = camNode.Fov;
            resource.viewportSize = (Vector2I)camera.GetViewport().GetVisibleRect().Size;
        }
    }

    public void ResetCamZoom()
    {
        zoom.ResetCamZoom(camNode, camera);
    }

    public void Process(double delta, TacticsCamera camera)
    {
        rotate.CheckFreeLookActivation(delta, camera);

        if (TacticsCameraResource.inFreeLook)
        {
            rotate.FreeLook(delta, camera.tPivot, camera.pPivot);
        }
        else if (!resource.isSnappingToQuad)
        {
            rotate.RotateCamera(delta, camera.tPivot, camera.pPivot);
        }

        Vector2 inputDirection = InputCaptureResource.camDirection;
        if (inputDirection != Vector2.Zero)
        {
            pan.WasdPan(delta, camera, inputDirection);
        }
        else if (pan.IsCursorNearEdge(camera) && !controls.isJoystick)
        {
            pan.EdgePan(delta, camera);
        }
        else
        {
            resource.panningTimer = 0.0f;
            move.StabilizeCamera(delta, camera);
        }

        if (camera.Velocity.Length() < MinVel)
        {
            camera.Velocity = Vector3.Zero;
        }

        move.FocusOnTarget(camera);
        zoom.ApplyZoomSmoothing(camera, delta);
    }
}
