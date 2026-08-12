using System;
using Godot;

[GlobalClass]
public partial class InputCaptureResource : Resource
{
    public static float rightStickX;
    public static float rightStickY;
    public static bool freeLookPressed = false;
    public const float ControllerDeadzone = 0.05f;
    public const float RightStickSensitivity = 1.0f;
}
