using System;
using System.Runtime.CompilerServices;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;
using Godot.Collections;

public partial class TacticsCameraRotationService : RefCounted
{
    public const int DeltaSmoothing = 10;
    public const int MaxVertRotation = 20;
    public const int MinVertRotation = -45;
    public const int FreeLookRotationFactor = 2;

    public TacticsCameraResource resource;
    public TacticsControlsResource controls;

    public TacticsCameraRotationService(
        TacticsCameraResource _resource,
        TacticsControlsResource _controls
    )
    {
        resource = _resource;
        controls = _controls;
    }

    public void FreeLook(double delta, Node3D tPivot, Node3D pPivot)
    {
        controls.SetCursorShapeToMoveHandler();

        Vector2 input = GetFreeLookInput();
        ApplyFreeLookRotation(input, delta, tPivot, pPivot);

        ResetTwistPutchInputs();
    }

    public void RotateCamera(double delta, Node3D tPivot, Node3D pPivot)
    {
        Quaternion currentQuatT = Quaternion.FromEuler(tPivot.Rotation);
        Quaternion currentQuatP = Quaternion.FromEuler(pPivot.Rotation);
        Vector3 destinationT = new Vector3(
            Mathf.DegToRad(TacticsCameraResource.xRotation),
            Mathf.DegToRad(TacticsCameraResource.yRotation),
            0
        );
        Vector3 destinationP = new Vector3(0, 0, resource.zRotation);
        Quaternion targetQuatT = Quaternion.FromEuler(destinationT);
        Quaternion targetQuatP = Quaternion.FromEuler(destinationP);

        Quaternion newQuatT = currentQuatT.Slerp(
            targetQuatT,
            resource.RotationSpeed * DeltaSmoothing * (float)delta
        );
        Quaternion newQuatP = currentQuatP.Slerp(
            targetQuatP,
            resource.RotationSpeed * DeltaSmoothing * (float)delta
        );

        tPivot.Rotation = newQuatT.GetEuler();
        pPivot.Rotation = newQuatP.GetEuler();

        if (Mathf.IsEqualApprox(tPivot.Rotation.Y, Mathf.DegToRad(TacticsCameraResource.yRotation)))
            resource.isRotating = false;
    }

    public void CheckFreeLookActivation(double delta, TacticsCamera camera)
    {
        if (controls.isJoystick)
        {
            if (IsJoystickInputActive())
            {
                TacticsCameraResource.inFreeLook = true;
                resource.freeLookTimer = 0.0f;
            }
            else if (TacticsCameraResource.inFreeLook)
            {
                UpdateFreeLookTimer(delta, camera);
            }
        }
        else
        {
            if (!InputCaptureResource.freeLookPressed && TacticsCameraResource.inFreeLook)
                DeactivateFreeLook(camera);
        }
    }

    public void DeactivateFreeLook(TacticsCamera camera)
    {
        TacticsCameraResource.inFreeLook = false;
        controls.SetCursorShapeToArrowHandler();
        SnapToNearestQuadrant(camera);
    }

    public void UpdateFreeLookTimer(double delta, TacticsCamera camera)
    {
        resource.freeLookTimer += (float)delta;
        if (
            resource.freeLookTimer >= TacticsCameraResource.FreeLookTimeout
            && TacticsCameraResource.inFreeLook
        )
        {
            DeactivateFreeLook(camera);
        }
    }

    public void AddAngleToHorizontalRotation(int twist)
    {
        if (twist != 0)
        {
            TacticsCameraResource.yRotation = (TacticsCameraResource.yRotation + twist) % 360;
            if (TacticsCameraResource.yRotation < 0)
            {
                TacticsCameraResource.yRotation += 360;
            }
        }
    }

    public Vector2 GetFreeLookInput()
    {
        return controls.isJoystick ? GetFreeLookJoystickInput() : GetFreeLookMouseInput();
    }

    public Vector2 GetFreeLookJoystickInput()
    {
        float rightStickX = InputCaptureResource.rightStickX;
        float rightStickY = InputCaptureResource.rightStickY;

        Vector2 input = Vector2.Zero;
        if (Math.Abs(rightStickX) > InputCaptureResource.rightStickX)
            input.X =
                -rightStickX * resource.RotationSpeed * InputCaptureResource.RightStickSensitivity;
        if (Math.Abs(rightStickY) > InputCaptureResource.rightStickY)
            input.Y =
                -rightStickY * resource.RotationSpeed * InputCaptureResource.RightStickSensitivity;

        return input;
    }

    public Vector2 GetFreeLookMouseInput()
    {
        return new Vector2(TacticsCameraResource.twistInput, TacticsCameraResource.pitchInput);
    }

    public void ApplyFreeLookRotation(Vector2 input, double delta, Node3D tPivot, Node3D pPivot)
    {
        tPivot.RotateY(input.X * FreeLookRotationFactor * (float)delta);
        pPivot.RotateX(input.Y * FreeLookRotationFactor * (float)delta);
        // I may have the X and Y Rotations swapped incorrectly here
        pPivot.Rotation = new Vector3(
            pPivot.Rotation.Y,
            Math.Clamp(
                pPivot.Rotation.X,
                Mathf.DegToRad(MinVertRotation),
                Mathf.DegToRad(MaxVertRotation)
            ),
            pPivot.Rotation.Z
        );
    }

    public void ResetTwistPutchInputs()
    {
        TacticsCameraResource.twistInput = 0.0f;
        TacticsCameraResource.pitchInput = 0.0f;
    }

    public bool IsJoystickInputActive()
    {
        float rightStickX = InputCaptureResource.rightStickX;
        float rightStickY = InputCaptureResource.rightStickY;
        return Math.Abs(rightStickX) > InputCaptureResource.ControllerDeadzone;
    }

    public void SnapToNearestQuadrant(TacticsCamera camera)
    {
        resource.isSnappingToQuad = true;
        Vector3 nearestQuadrant = CalculateNearestQuadrant(camera);

        Vector3 currentRotation = camera.tPivot.RotationDegrees;
        Vector3 targetRotation = nearestQuadrant;

        float rotationDifference = targetRotation.Y - currentRotation.Y;
        if (Math.Abs(rotationDifference) > 180)
        {
            if (rotationDifference > 0)
                targetRotation.Y -= 360;
            else
                targetRotation.Y += 360;
        }

        Tween tween = camera.CreateTween();
        tween
            .TweenProperty(
                camera.tPivot,
                "RotationDegrees",
                targetRotation,
                resource.quadSnapDuration
            )
            .SetTrans(Tween.TransitionType.Sine);
        tween
            .Parallel()
            .TweenProperty(
                camera.pPivot,
                "RotationDegrees:X",
                resource.zRotation,
                resource.quadSnapDuration
            )
            .SetTrans(Tween.TransitionType.Sine);
        tween.TweenCallback(
            Callable.From(
                (TacticsCamera camera) =>
                {
                    camera.tPivot.RotationDegrees = new Vector3(
                        camera.tPivot.RotationDegrees.Y % 360.0f,
                        camera.tPivot.RotationDegrees.X,
                        camera.tPivot.RotationDegrees.Z
                    );
                    if (camera.tPivot.RotationDegrees.Y < 0)
                        new Vector3(
                            camera.tPivot.RotationDegrees.Y + 360.0f,
                            camera.tPivot.RotationDegrees.X,
                            camera.tPivot.RotationDegrees.Z
                        );
                    resource.isSnappingToQuad = false;
                }
            )
        );

        TacticsCameraResource.yRotation = (int)targetRotation.Y % 360;
        if (TacticsCameraResource.yRotation < 0)
        {
            TacticsCameraResource.yRotation += 360;
        }
    }

    public Vector3 CalculateNearestQuadrant(TacticsCamera camera)
    {
        float currentRotation = camera.tPivot.RotationDegrees.Y;
        Array<int> quadrants = [45, 135, 225, 315];

        currentRotation = currentRotation % 360.0f;
        if (currentRotation < 0)
            currentRotation += 360.0f;

        int nearestQuadrant = 0;
        int smallestDifference = 360;

        foreach (int quadrant in quadrants)
        {
            float difference = Math.Abs(currentRotation - quadrant);
            difference = Math.Min(difference, 360 - difference);
            if (difference < smallestDifference)
            {
                smallestDifference = (int)Math.Round(difference);
                nearestQuadrant = (int)Math.Round((float)quadrant);
            }
        }

        return new Vector3(TacticsCameraResource.xRotation, nearestQuadrant, 0);
    }
}
