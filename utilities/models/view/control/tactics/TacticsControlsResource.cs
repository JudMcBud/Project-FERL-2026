using System.Collections.Generic;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

namespace Game.Models.View.Control.Tactics.TacticsControlsResource;

[GlobalClass]
public partial class TacticsControlsResource : Resource
{
    [Signal]
    public delegate void SetActionsMenuVisibilityEventHandler(bool v, TacticsPawn pawn);

    [Signal]
    public delegate void MoveCameraEventHandler(double delta);

    [Signal]
    public delegate void SelectPawnEventHandler(TacticsPlayer player);

    [Signal]
    public delegate void SelectPawnToAttackEventHandler();

    [Signal]
    public delegate void SelectNewLocationEventHandler();

    [Signal]
    public delegate void SetCursorShapeToMoveEventHandler();

    [Signal]
    public delegate void SetCursorShapeToArrowEventHandler();

    [Export]
    public bool isJoystick;

    // Should probably phase this out at the end
    [Export]
    public bool inputHintsFolded;

    public Dictionary<string, string> actions = new Dictionary<string, string>
    {
        { "Move", "PlayerWantsToMove" },
        { "Wait", "PlayerWantsToWait" },
        { "Cancel", "PlayerWantsToCancel" },
        { "Attack", "PlayerWantsToAttack" },
        { "DebugNextTurn", "PLayerWantsToSkipTurn" },
    };

    public void SetActionsMenuVisibilityHandler(bool v, TacticsPawn pawn)
    {
        EmitSignal(SignalName.SetActionsMenuVisibility, v, pawn);
    }

    public void MoveCameraHandler(double delta)
    {
        EmitSignal(SignalName.MoveCamera, delta);
    }

    public void SelectPawnHandler(TacticsPlayer player)
    {
        EmitSignal(SignalName.SelectPawn, player);
    }

    public void SelectPawnToAttackHandler()
    {
        EmitSignal(SignalName.SelectPawnToAttack);
    }

    public void SelectNewLocationHandler()
    {
        EmitSignal(SignalName.SelectNewLocation);
    }

    public void SetCursorShapeToMoveHandler()
    {
        EmitSignal(SignalName.SetCursorShapeToMove);
    }

    public void SetCursorShapeToArrowHandler()
    {
        EmitSignal(SignalName.SetCursorShapeToArrow);
    }
}
