using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Godot;

public partial class TacticsControlsCameraService : RefCounted
{
    TacticsCameraResource tCam;

    public TacticsControlsCameraService(TacticsCameraResource _tCam)
    {
        tCam = _tCam;
    }

    public void MoveCamera(double delta, bool isJoystick)
    {
        float h = -Input.GetActionStrength("cameraLeft") + Input.GetActionStrength("cameraRight");
        float v =
            Input.GetActionStrength("cameraForward") - Input.GetActionStrength("cameraBackwards");

        tCam.MoveCameraHandler(h, v, isJoystick, delta);
    }
}
