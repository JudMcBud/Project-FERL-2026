using System;
using Godot;

public partial class InputCapture : Node3D
{
    [Export]
    InputCaptureResource resource;
    public InputCaptureService service;

    public InputCapture()
    {
        resource = GD.Load<InputCaptureResource>(
            "res://utilities/models/view/control/input/capture/InputCaptureResource.tres"
        );
        service = new InputCaptureService(resource);
    }

    public void Input(InputEvent e)
    {
        service.ProcessInput(e);
    }

    public void Process(double delta)
    {
        ProjectMousePosition(1, false);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        service.HandleInput(@event);
    }

    public CollisionObject3D ProjectMousePosition(int collisionMask, bool isJoystick)
    {
        return service.ProjectMousePosition(collisionMask, isJoystick, this);
    }
}
