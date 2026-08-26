using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

public partial class TacticsCamera : CharacterBody3D
{
    [Export]
    public TacticsCameraResource resource;

    [Export]
    public TacticsControlsResource controls;

    public static TacticsCameraService service;

    private static TacticsCameraResource LoadCameraResource(string path)
    {
        return ResourceLoader.Load<TacticsCameraResource>(path) ?? new TacticsCameraResource();
    }

    private static TacticsControlsResource LoadControlsResource(string path)
    {
        return ResourceLoader.Load<TacticsControlsResource>(path) ?? new TacticsControlsResource();
    }

    public Node3D tPivot;
    public Node3D pPivot;

    public Camera3D camNode;

    public override void _Ready()
    {
        resource = LoadCameraResource("res://utilities/models/view/camera/tactics/camera.tres");
        controls = LoadControlsResource("res://utilities/models/view/control/tactics/control.tres");

        tPivot = GetNode<Node3D>("TwistPivot");
        pPivot = GetNode<Node3D>("TwistPivot/PitchPivot");
        camNode = GetNode<Camera3D>("TwistPivot/PitchPivot/Camera3D");

        service = new TacticsCameraService(resource, controls);
        service.Setup(this, camNode);
        resource.boundaryCenter = GlobalPosition;

        resource.RotateCamera += RotateCamera;
        resource.MoveCamera += MoveCamera;
    }

    public override void _Process(double delta)
    {
        service.Process(delta, this);
    }

    public void MoveCamera(float h, float v, bool joystick, double delta)
    {
        service.move.MoveCamera(h, v, joystick, delta, this);
    }

    public void RotateCamera(double delta, int twist = 0)
    {
        TacticsCameraResource.isRotating = true;
        service.rotate.AddAngleToHorizontalRotation(twist);
        service.rotate.RotateCamera(delta, tPivot, pPivot);
    }

    public void FreeLook(double delta)
    {
        service.rotate.FreeLook(delta, tPivot, pPivot);
    }

    public static void ZoomCamera(float zoomIncrement)
    {
        service.zoom.ZoomCamera(zoomIncrement);
    }

    public void ResetCamZoom()
    {
        service.ResetCamZoom();
    }
}
