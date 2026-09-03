using System;
using System.Diagnostics;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;
using Godot.Collections;

public partial class TacticsControlsService : RefCounted
{
    public TacticsControlsResource controls;
    public TacticsCameraResource tacticsCam;
    public TacticsParticipantResource participant;
    public TacticsArenaResource arena;
    public Node inputCapture;
    public TacticsControlsInputService inputService;
    public TacticsUIService uiService;
    public TacticsControlsCameraService cameraService;
    public TacticsControlsSelectionService pawnSelectionService;

    public TacticsControlsService(
        TacticsControlsResource _controls,
        TacticsCameraResource _tacticsCam,
        TacticsParticipantResource _participant,
        TacticsArenaResource _arena,
        Node _inputCapture
    )
    {
        controls = _controls;
        tacticsCam = _tacticsCam;
        participant = _participant;
        arena = _arena;
        inputCapture = _inputCapture;
        inputService = new TacticsControlsInputService(controls, inputCapture);
        uiService = new TacticsUIService(controls);
        cameraService = new TacticsControlsCameraService(tacticsCam);
        pawnSelectionService = new TacticsControlsSelectionService(
            participant,
            arena,
            controls,
            tacticsCam,
            inputService
        );
    }

    public void Setup(TacticsControls ctrl)
    {
        if (controls == null)
            GD.PushError(
                "TacticsControls needs a ControlResource from /models/view/controls/tactics/"
            );
        else
        {
            controls.SetActionsMenuVisibility += ctrl.SetActionsMenuVisibility;
            controls.SetCursorShapeToMove += ctrl.SetCursorShapeToMove;
            controls.SetCursorShapeToArrow += ctrl.SetCursorShapeToArrow;
            controls.SelectPawn += ctrl.SelectPawn;
            controls.SelectPawnToAttack += ctrl.SelectPawnToAttack;
            controls.SelectNewLocation += ctrl.SelectNewLocation;
        }
        if (tacticsCam == null)
            GD.PushError(
                "TacticsCamera needs a CameraResource (T Cam) from /models/view/camera/tactics/"
            );
        if (arena == null)
            GD.PushError("TacticsControls needs an ArenaResource from /models/world/combat/arena/");
    }

    public void PhysicsProcess(double delta, TacticsControls ctrl, Array<Node> labels)
    {
        inputService.UpdateMouseMode();
        uiService.UpdateControllerHints(ctrl);
        pawnSelectionService.PhysicsProcess(ctrl, labels);
    }

    public void HandleInput(InputEvent e)
    {
        inputService.HandleInput(e);
    }

    public void SetActionsMenuVisibility(bool v, TacticsPawn p, TacticsControls ctrl)
    {
        uiService.SetActionsMenuVisibility(v, p, ctrl);
    }

    public void SelectPawn(TacticsPlayer player, TacticsControls ctrl)
    {
        pawnSelectionService.SelectPawn(player, ctrl);
    }

    public void SelectNewLocation(TacticsControls ctrl)
    {
        pawnSelectionService.SelectNewLocation(ctrl);
    }

    public void SelectPawnToAttack(TacticsControls ctrl)
    {
        pawnSelectionService.SelectPawnToAttack(ctrl);
    }

    public void PlayerWantsToMove()
    {
        pawnSelectionService.PlayerWantsToMove();
    }

    public void PlayerWantsToCancel()
    {
        pawnSelectionService.PlayerWantsToCancel();
    }

    public void PlayerWantsToWait()
    {
        pawnSelectionService.PlayerWantsToWait();
    }

    public void PlayerWantsToSkipTurn()
    {
        pawnSelectionService.PlayerWantsToSkipTurn();
    }

    public void PlayerWantsToAttack()
    {
        pawnSelectionService.PlayerWantsToAttack();
    }
}
