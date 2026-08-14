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
        double delta
    );

    [Signal]
    public delegate void FreeLookEventHandler(double delta, float twist);

    [Signal]
    public delegate void RotateCameraEventHandler(double delta, int twist = 0);

    #region Movement
    [ExportCategory("Movement")]
    [Export(PropertyHint.Range, "1,100,")]
    public int moveSpeed { get; set; } = 10;
    public static float rotSpeed;

    [Export(PropertyHint.Range, "1,100,")]
    public float RotationSpeed
    {
        get => field;
        set => field = (float)value / 10.0f;
    }

    [Export(PropertyHint.Range, "0.01, 1,")]
    public float smoothing = 0.1f;

    public Vector3 targetVelocity = Vector3.Zero;

    public Node3D target = null;
    #endregion

    #region Zoom
    [ExportCategory("Zoom")]
    public static float zoomSpeed = 0.5f;

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

    public const float PanningDelay = 0.05f;
    public float panningTimer = 0.0f;
    #endregion

    #region Rotation
    [ExportCategory("Rotation")]
    //Probably want to turn quadrant snapping off later
    [Export(PropertyHint.Range, "0.1, 10,")]
    public float quadSnapDuration = 0.2f;

    public bool isSnappingToQuad = false;
    public static bool isRotating = false;
    public static int xRotation;

    [Export]
    public int VerticalRotation
    {
        get => xRotation;
        set => xRotation = value;
    }

    public static int yRotation;

    [Export]
    public int HorizontalRotation
    {
        get => yRotation;
        set => yRotation = value;
    }

    [Export]
    public int zRotation;

    public Vector2 mousePosition;

    public static bool inFreeLook;

    // Probably want to change this to a recenter camera button press input instead of a timer
    public float freeLookTimer = 0.0f;
    public const float FreeLookTimeout = 0.05f;
    public static float twistInput;
    public static float pitchInput;
    public Vector2I viewportSize;
    #endregion

    #region Signals

    /// <summary>
    /// Emits signal to move camera
    /// </summary>
    public void MoveCameraHandler(float horizontal, float vertical, bool joystick, double delta)
    {
        EmitSignal(SignalName.MoveCamera, horizontal, vertical, joystick, delta);
    }

    private void RotateCameraHandler(double delta, int twist = 0)
    {
        EmitSignal(SignalName.RotateCamera, delta, twist);
    }

    private void FreeLookHandler(double delta)
    {
        EmitSignal(SignalName.FreeLook, delta);
    }
    #endregion
}
