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
    private delegate void MoveCameraEventHandler(double delta);

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
