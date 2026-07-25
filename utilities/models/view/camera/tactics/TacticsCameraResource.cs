using System;
using Godot;

namespace Game.Models.View.Camera.Tactics.TacticsCameraResource;

[GlobalClass]
public partial class TacticsCameraResource : Resource
{
    [Signal]
    public delegate void MoveCameraEventHandler(
        float horizontal,
        float vertical,
        bool joystick,
        float delta
    );

    [Signal]
    public delegate void FreeLookEventHandler(float delta, float twist);

    [Signal]
    public delegate void RotateCameraEventHandler();

    #region Movement
    [ExportCategory("Movement")]
    [Export(PropertyHint.Range, "1,100,")]
    public int moveSpeed { get; set; } = 10;
    private static float rotSpeed;

    [Export(PropertyHint.Range, "1,100,")]
    public float RotationSpeed
    {
        get => field;
        set => field = (float)value / 10.0f;
    }

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float smoothing = 0.1f;

    private Vector3 targetVelocity = Vector3.Zero;

    private Node3D target = null;
    #endregion

    #region Zoom
    [ExportCategory("Zoom")]
    private static float zoomSpeed = 0.5f;

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float CameraZoomSpeed
    {
        get => zoomSpeed;
        set => zoomSpeed = (float)value;
    }

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float zoomSmoothness = 0.1f;

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float zoomDuration = 0.5f;

    [Export(PropertyHint.Range, "0.1, 50,")]
    public float minZoom = 1.0f;

    [Export(PropertyHint.Range, "10.0, 100,")]
    public float maxZoom = 10.0f;

    public float currentFOV = 50.0f;
    public float targetFOV = 50.0f;
    #endregion

    #region Panning
    [ExportCategory("Panning")]
    [Export]
    public float boundaryRadius = 10.0f;
    public Vector3 boundaryCenter = Vector3.Zero;

    [Export(PropertyHint.Range, "1, 50,")]
    public float borderPanPixelThreshold = 1.0f;

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float mousePanSpeed = 0.5f;

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float joyPanSpeed = 0.5f;

    private const float PanningDelay = 0.05f;
    private const float panningTimer = 0.0f;
    #endregion

    #region Rotation
    [ExportCategory("Rotation")]
    //Probably want to turn quadrant snapping off later
    [Export(PropertyHint.Range, "0.1, 10,")]
    public float quadSnapDuration = 0.2f;

    private bool isSnappingToQuad = false;
    private bool isRotating = false;
    private static int xRotation;

    [Export]
    public int VerticalRotation
    {
        get => xRotation;
        set => xRotation = value;
    }

    private static int yRotation;

    [Export]
    public int HorizontalRotation
    {
        get => yRotation;
        set => yRotation = value;
    }

    [Export]
    public int zRotation;

    private Vector2 mousePosition;

    private static bool inFreeLook;

    // Probably want to change this to a recenter camera button press input instead of a timer
    private float freeLookTimer = 0.0f;
    private const float FreeLookTimeout = 0.05f;
    private static float twistInput;
    private static float pitchInput;
    private Vector2I viewportSize;
    #endregion

    #region Signals
    private void MoveCamera(float horizontal, float vertical, bool joystick, float delta)
    {
        EmitSignal(SignalName.MoveCamera, horizontal, vertical, joystick, delta);
    }

    private void RotateCamera(float delta, float twist = 0.0f)
    {
        EmitSignal(SignalName.RotateCamera, delta, twist);
    }

    private void FreeLook(float delta)
    {
        EmitSignal(SignalName.FreeLook, delta);
    }
    #endregion
}
