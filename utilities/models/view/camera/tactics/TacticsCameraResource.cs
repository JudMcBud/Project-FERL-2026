using System;
using Godot;

namespace Game.Models.View.Camera.Tactics.TacticsCameraResource;

[GlobalClass]
public partial class TacticsCameraResource : Resource
{
    [Signal]
    public delegate void CalledMoveCameraEventHandler();

    [Signal]
    public delegate void CalledFreeLookEventHandler();

    [Signal]
    public delegate void CalledRotateCameraEventHandler();

    #region Movement
    [ExportCategory("Movement")]
    [Export(PropertyHint.Range, "1,100,")]
    public int moveSpeed { get; set; } = 10;
    private static float rotSpeed;

    [Export(PropertyHint.Range, "1,100,")]
    public float RotationSpeed
    {
        get { return rotSpeed; }
        set { rotSpeed = (float)value / 10.0f; }
    }

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float smoothing = 0.1f;

    private Vector3 targetVelocity = Vector3.Zero;

    private Node3D target = null;
    #endregion

    #region Zoom
    [ExportCategory("Zoom")]
    private static float zoomSpeed;

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float CameraZoomSpeed
    {
        get => field;
        set => field = (float)value;
    } = 0.5f;
    #endregion
    #region Panning
    [ExportCategory("Panning")]
    [Export]
    public float boundaryRadius = 10.0f;
    #endregion
}
