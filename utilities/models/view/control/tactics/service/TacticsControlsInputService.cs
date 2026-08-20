using System;
using System.Collections.Generic;
using Game.Models.Config.TacticsConfig;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

public partial class TacticsControlsInputService : RefCounted
{
    public TacticsControlsResource controls;
    public MouseClickCapture inputCapture;

    public TacticsControlsInputService(TacticsControlsResource _controls, Node _inputCapture)
    {
        controls = _controls;
        inputCapture = (MouseClickCapture)_inputCapture;
    }

    public void UpdateMouseMode()
    {
        // May need to adjust mouse modes
        Input.MouseModeEnum mouseMode = controls.isJoystick
            ? Input.MouseModeEnum.Hidden
            : Input.MouseModeEnum.Visible;
        Input.MouseMode = mouseMode;
    }

    public void HandleInput(InputEvent e)
    {
        controls.isJoystick = e is InputEventJoypadButton || e is InputEventJoypadMotion;
        GD.Print(
            $"[TacticsControlsInputService] {e.GetType().Name} received; "
                + $"device={(controls.isJoystick ? "Controller" : "Mouse/Keyboard")}"
        );
    }

    public Object Get3DCanvasMousePosition(int collisionMask, TacticsControls ctrl)
    {
        if (IsMouseHoveringUIElem(ctrl))
            return null;
        if (inputCapture != null)
            return inputCapture.ProjectMousePosition(collisionMask, controls.isJoystick);
        else
            return null;
    }

    public bool IsMouseHoveringUIElem(TacticsControls ctrl, List<string> elm = null)
    {
        if (elm == null)
            elm = TacticsConfig.uiElem;

        foreach (string e in elm)
        {
            if (ctrl.GetNode<Control>(e).Visible)
            {
                switch (e)
                {
                    case "%Actions":
                        foreach (Button action in ctrl.GetNode<Control>(e).GetChildren())
                        {
                            if (
                                action
                                    .GetGlobalRect()
                                    .HasPoint(ctrl.GetViewport().GetMousePosition())
                            )
                                return true;
                        }
                        break;
                    case "%Hints":
                        foreach (TextureRect hint in ctrl.GetNode<Control>(e).GetChildren())
                        {
                            if (
                                hint.GetGlobalRect().HasPoint(ctrl.GetViewport().GetMousePosition())
                            )
                                return true;
                        }
                        break;
                }
            }
        }
        return false;
    }
}
