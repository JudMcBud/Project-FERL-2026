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
    public delegate void MoveCameraEventHandler(float delta);

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

    private void SetActionsMenuVisibility(bool v, TacticsPawn pawn)
    {
        EmitSignal(SignalName.SetActionsMenuVisibility, v, pawn);
    }

    private void MoveCamera(float delta)
    {
        EmitSignal(SignalName.MoveCamera, delta);
    }

    private void SelectPawn(TacticsPlayer player)
    {
        EmitSignal(SignalName.SelectPawn, player);
    }

    private void SelectPawnToAttack()
    {
        EmitSignal(SignalName.SelectPawnToAttack);
    }

    private void SelectNewLocation()
    {
        EmitSignal(SignalName.SelectNewLocation);
    }

    private void SetCursorShapeToMove()
    {
        EmitSignal(SignalName.SetCursorShapeToMove);
    }

    private void SetCursorShapeToArrow()
    {
        EmitSignal(SignalName.SetCursorShapeToArrow);
    }
}
