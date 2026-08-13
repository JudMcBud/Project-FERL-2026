using System;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsUIService : RefCounted
{
    public TacticsControlsResource controls;

    public TacticsUIService(TacticsControlsResource _controls)
    {
        controls = _controls;
    }

    public void UpdateControllerHints(TacticsControls ctrl)
    {
        if (controls.isJoystick)
        {
            ctrl.GetNode<TextureRect>("ControllerHints").Texture = ctrl.layoutXbox;
        }
        else
        {
            ctrl.GetNode<TextureRect>("ControllerHints").Texture = ctrl.layoutPC;
        }
    }

    public void SetActionsMenuVisibility(bool v, TacticsPawn p, TacticsControls ctrl)
    {
        if (!ctrl.GetNode<BoxContainer>("HBoxContainer/Actions").Visible)
        {
            ctrl.GetNode<Button>("HBox/Actions/Move").GrabFocus();
        }

        ctrl.GetNode<BoxContainer>("HBox/Actions").Visible = v && p.CanAct();

        if (p == null)
            return;

        ctrl.GetNode<Button>("HBox/Actions/Move").Disabled = !p.resource.canMove;
        ctrl.GetNode<Button>("HBox/Actions/Attack").Disabled = !p.resource.canAttack;
    }
}
