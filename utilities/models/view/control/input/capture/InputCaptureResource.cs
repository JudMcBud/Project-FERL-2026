using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class InputCaptureResource : Resource
{
    #region Input State
    public static bool isJoystick = false;
    public Vector2 joystickPosition = Vector2.Zero;
    public static float rightStickX;
    public static float rightStickY;
    public static float leftStickX;
    public static float leftStickY;

    public Vector2 mousePosition = Vector2.Zero;
    public static Vector2 camDirection;
    public static bool freeLookPressed = false;
    #endregion

    #region Mapping
    public static readonly Array<string> CameraPanKeys =
    [
        "cameraLeft",
        "cameraRight",
        "cameraForwards",
        "cameraBackwards",
    ];
    #endregion

    #region Configuration
    public const int RayLength = 10000;

    [Export]
    public float mouseSensitivity = 1.0f;

    public const float ControllerDeadzone = 0.05f;
    public const float RightStickSensitivity = 1.0f;

    [Export]
    public float cameraMoveSpeed = 10.0f;

    [Export]
    public float cameraRotateSpeed = 5.0f;

    [Export]
    public float cameraZoomSpeed = 0.1f;
    #endregion
}
