using System;
using System.Net;
using System.Runtime.CompilerServices;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Godot;

public partial class InputCaptureService : RefCounted
{
    InputCaptureResource resource;
    public float FlRotSpeedDivider = 0.25f;

    public InputCaptureService(InputCaptureResource _resource)
    {
        resource = _resource;
    }

    public void ProcessInput(InputEvent e)
    {
        if (e is InputEventMouseButton mouseButton)
        {
            // --- Mouse Buttons ---
            if (mouseButton.IsPressed())
            {
                // Free look toggle
                if (mouseButton.IsAction("cameraFreeLook"))
                {
                    InputCaptureResource.freeLookPressed = true;
                    if (!TacticsCameraResource.isRotating)
                        TacticsCameraResource.inFreeLook = true;
                }
                // Zoom input -- camea zoom scroll capture
                if (mouseButton.ButtonIndex == MouseButton.WheelUp)
                    TacticsCamera.service.zoom.ZoomCamera(-TacticsCameraResource.zoomSpeed);
                else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
                    TacticsCamera.service.zoom.ZoomCamera(TacticsCameraResource.zoomSpeed);
            }
            else
            {
                // Free look toggle
                if (e.IsActionReleased("cameraFreeLook"))
                    InputCaptureResource.freeLookPressed = false;
            }
        }
        if (e is InputEventMouseMotion mouseMotion)
        {
            // Free look motion capture
            if (TacticsCameraResource.inFreeLook)
            {
                TacticsCameraResource.twistInput =
                    -mouseMotion.Relative.X * (FlRotSpeedDivider * TacticsCameraResource.rotSpeed);
                TacticsCameraResource.pitchInput =
                    -mouseMotion.Relative.Y * (FlRotSpeedDivider * TacticsCameraResource.rotSpeed);
            }
        }
        else if (e is InputEventKey key)
        {
            // --- Keys ---
            if (key.Pressed)
            {
                if (key.IsActionPressed("cameraRotateLeft"))
                {
                    if (!TacticsCameraResource.inFreeLook)
                        TacticsCameraResource.yRotation += -90;
                }
                else if (key.IsActionPressed("cameraRotateRight"))
                {
                    if (!TacticsCameraResource.inFreeLook)
                        TacticsCameraResource.yRotation += 90;
                }
                // Camera pan direction (WASD)
                foreach (StringName action in InputCaptureResource.CameraPanKeys)
                {
                    if (key.IsAction(action))
                    {
                        InputCaptureResource.camDirection = Input.GetVector(
                            "cameraLeft",
                            "cameraRight",
                            "cameraForward",
                            "cameraBackwards"
                        );
                        return;
                    }
                }
            }
            else
            {
                // Camera pan direction (WASD)
                foreach (StringName action in InputCaptureResource.CameraPanKeys)
                {
                    if (key.IsActionReleased(action))
                    {
                        // Recalculate camDirection after key release
                        InputCaptureResource.camDirection = Input.GetVector(
                            "cameraLeft",
                            "cameraRight",
                            "cameraForward",
                            "cameraBackwards"
                        );
                        return;
                    }
                }
            }
        }
        else if (e is InputEventJoypadMotion joypadMotion)
        {
            // --- Joysticks ---
            // Camera pan direction (left joystick)
            if (joypadMotion.Axis is JoyAxis.LeftX || joypadMotion.Axis is JoyAxis.LeftY)
            {
                InputCaptureResource.leftStickX = Input.GetJoyAxis(0, JoyAxis.LeftX);
                InputCaptureResource.leftStickY = Input.GetJoyAxis(0, JoyAxis.LeftY);

                // Calculate the magnitude of the joystick input
                float magnitude = new Vector2(
                    InputCaptureResource.leftStickX,
                    InputCaptureResource.leftStickY
                ).Length();
                if (magnitude > InputCaptureResource.ControllerDeadzone)
                    InputCaptureResource.camDirection = new Vector2(
                        InputCaptureResource.leftStickX,
                        InputCaptureResource.leftStickY
                    );
                else
                    InputCaptureResource.camDirection = Vector2.Zero;
            }
            // Camera free look direction (right joystick)
            if (joypadMotion.Axis is JoyAxis.RightX || joypadMotion.Axis is JoyAxis.RightY)
            {
                if (Math.Abs(joypadMotion.AxisValue) > InputCaptureResource.ControllerDeadzone)
                {
                    InputCaptureResource.rightStickX = -Input.GetJoyAxis(0, JoyAxis.RightX);
                    InputCaptureResource.rightStickY = -Input.GetJoyAxis(0, JoyAxis.RightY);
                }
                else if (Math.Abs(joypadMotion.AxisValue) < InputCaptureResource.ControllerDeadzone)
                {
                    InputCaptureResource.rightStickX = 0.0f;
                    InputCaptureResource.rightStickY = 0.0f;
                }
            }
        }
        else if (e is InputEventJoypadButton joypadButton)
        {
            // --- Joy Buttons ---
            if (joypadButton.Pressed)
            {
                // Future functionality can go here
            }
            else
            {
                // Future functionality can go here
            }
        }
    }

    public void HandleInput(InputEvent e)
    {
        // Future functionality can go here
    }

    public CollisionObject3D ProjectMousePosition(
        int collisionMask,
        bool isJoystick,
        InputCapture inputCapture
    )
    {
        if (resource == null)
            return null;
        Camera3D camera = inputCapture.GetViewport().GetCamera3D();
        resource.mousePosition = inputCapture.GetViewport().GetMousePosition();
        Vector2 pointerOrigin = !isJoystick
            ? resource.mousePosition
            : inputCapture.GetViewport().GetVisibleRect().Size / 2;

        Vector3 from = camera.ProjectRayOrigin(pointerOrigin);
        Vector3 to =
            from + camera.ProjectLocalRayNormal(pointerOrigin) * InputCaptureResource.RayLength;

        PhysicsRayQueryParameters3D rayQuery = PhysicsRayQueryParameters3D.Create(
            from,
            to,
            (uint)collisionMask,
            []
        );
        CollisionObject3D collider = (CollisionObject3D)
            inputCapture.GetWorld3D().DirectSpaceState.IntersectRay(rayQuery)["collider"];

        return collider;
    }
}
