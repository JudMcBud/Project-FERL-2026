using System.Collections.Generic;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

namespace Game.Models.View.Control.Tactics.TacticsControlsResource;

[GlobalClass]
public partial class TacticsControlsResource : Resource
{
    [Signal]
    private delegate void SetActionsMenuVisibilityEventHandler(bool v, TacticsPawn pawn);

    [Signal]
    private delegate void MoveCameraEventHandler(float delta);

    [Signal]
    private delegate void SelectPawnEventHandler(TacticsPlayer player);

    [Signal]
    private delegate void SelectPawnToAttackEventHandler();

    [Signal]
    private delegate void SelectNewLocationEventHandler();

    [Signal]
    private delegate void SetCursorShapeToMoveEventHandler();

    [Signal]
    private delegate void SetCursorShapeToArrowEventHandler();

    [Export]
    private bool isJoystick;

    // Should probably phase this out at the end
    [Export]
    private bool inputHintsFolded;

    public Dictionary<string, string> actions = new Dictionary<string, string>
    {
        { "Move", "PlayerWantsToMove" },
        { "Wait", "PlayerWantsToWait" },
        { "Cancel", "PlayerWantsToCancel" },
        { "Attack", "PlayerWantsToAttack" },
        { "DebugNextTurn", "PLayerWantsToSkipTurn" },
    };

    private void SetActionsMenuVisibilityHandler(bool v, TacticsPawn pawn)
    {
        EmitSignal(SignalName.SetActionsMenuVisibility, v, pawn);
    }

    private void MoveCameraHandler(float delta)
    {
        EmitSignal(SignalName.MoveCamera, delta);
    }

    private void SelectPawnHandler(TacticsPlayer player)
    {
        EmitSignal(SignalName.SelectPawn, player);
    }

    private void SelectPawnToAttackHandler()
    {
        EmitSignal(SignalName.SelectPawnToAttack);
    }

    private void SelectNewLocationHandler()
    {
        EmitSignal(SignalName.SelectNewLocation);
    }

    private void SetCursorShapeToMoveHandler()
    {
        EmitSignal(SignalName.SetCursorShapeToMove);
    }

    private void SetCursorShapeToArrowHandler()
    {
        EmitSignal(SignalName.SetCursorShapeToArrow);
    }
}
