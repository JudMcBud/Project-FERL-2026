using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsControlsSelectionService : RefCounted
{
    public TacticsParticipantResource participant;
    public TacticsArenaResource arena;
    public TacticsControlsResource controls;
    public TacticsCameraResource tCam;
    public TacticsControlsInputService inputService;

    public TacticsControlsSelectionService(
        TacticsParticipantResource _participant,
        TacticsArenaResource _arena,
        TacticsControlsResource _controls,
        TacticsCameraResource _tCam,
        TacticsControlsInputService _inputService
    )
    {
        participant = _participant;
        arena = _arena;
        controls = _controls;
        tCam = _tCam;
        inputService = _inputService;
    }

    public void SelectPawn(TacticsPlayer player, TacticsControls ctrl)
    {
        arena.OnResetAllTileMarkers();
        if (ctrl.currentPawn != null)
        {
            controls.SetActionsMenuVisibilityHandler(false, participant.currentPawn);
            ctrl.currentPawn.ShowPawnStats(false);
        }

        ctrl.currentPawn = SelectHoveredPawn(ctrl);
        if (ctrl.currentPawn == null)
            return;
        else
            ctrl.currentPawn.ShowPawnStats(true);

        if (Input.IsActionPressed("uiAccept") && ctrl.currentPawn.CanAct())
        {
            if (player.GetChildren().Contains(ctrl.currentPawn))
            {
                tCam.target = ctrl.currentPawn;
                participant.currentPawn = ctrl.currentPawn;
                controls.SetActionsMenuVisibilityHandler(true, participant.currentPawn);
                participant.stage = TacticsParticipantResource.Stage.ShowActions;
            }
        }
    }

    public TacticsPawn SelectHoveredPawn(TacticsControls ctrl)
    {
        TacticsPawn pawn = (TacticsPawn)inputService.Get3DCanvasMousePosition(2, ctrl);
        TacticsTile tile =
            pawn == null
                ? (TacticsTile)inputService.Get3DCanvasMousePosition(1, ctrl)
                : pawn.GetTile();
        arena.OnMarkHoverTile(tile);
        if (pawn != null)
            return pawn;
        else if (tile != null)
            return (TacticsPawn)tile.GetTileOccupier();
        else
            return null;
    }

    public TacticsTile SelectHoveredTile(TacticsControls ctrl)
    {
        TacticsPawn pawn = (TacticsPawn)inputService.Get3DCanvasMousePosition(2, ctrl);
        TacticsTile tile =
            pawn == null
                ? (TacticsTile)inputService.Get3DCanvasMousePosition(1, ctrl)
                : pawn.GetTile();
        arena.OnMarkHoverTile(tile);
        return tile;
    }

    public void SelectNewLocation(TacticsControls ctrl)
    {
        TacticsTile tile = (TacticsTile)inputService.Get3DCanvasMousePosition(1, ctrl);
        arena.OnMarkHoverTile(tile);
        if (Input.IsActionPressed("uiAccept") && tile != null && tile.reachable)
        {
            ctrl.currentPawn.resource.pathfindingTilestack = arena.OnGetPathfindingTileStack(tile);
            tCam.target = tile;
            participant.stage = TacticsParticipantResource.Stage.MovePawn;
        }
    }

    public void SelectPawnToAttack(TacticsControls ctrl)
    {
        controls.SetActionsMenuVisibilityHandler(true, participant.currentPawn);
        if (participant.attackablePawn != null)
        {
            controls.SetActionsMenuVisibilityHandler(false, participant.attackablePawn);
            participant.attackablePawn.ShowPawnStats(false);
        }
        TacticsTile tile = SelectHoveredTile(ctrl);
        participant.attackablePawn = tile != null ? (TacticsPawn)tile.GetTileOccupier() : null;
        if (participant.attackablePawn != null)
        {
            controls.SetActionsMenuVisibilityHandler(true, participant.attackablePawn);
            participant.attackablePawn.ShowPawnStats(true);
        }
        if (Input.IsActionJustPressed("uiAccept") && tile != null && tile.attackable)
        {
            tCam.target = participant.attackablePawn;
            participant.stage = TacticsParticipantResource.Stage.Attack;
        }
    }

    public void PlayerWantsToMove()
    {
        if (participant.displayOpponentStats)
            participant.displayOpponentStats = false;
        participant.stage = TacticsParticipantResource.Stage.ShowMovements;
    }

    public void PlayerWantsToCancel()
    {
        if (participant.displayOpponentStats)
            participant.displayOpponentStats = false;
        if (
            participant.stage != TacticsParticipantResource.Stage.ShowActions
            && participant.stage != TacticsParticipantResource.Stage.SelectPawn
        )
            participant.stage = TacticsParticipantResource.Stage.ShowActions;
        else
            participant.stage = TacticsParticipantResource.Stage.SelectPawn;
    }

    public void PlayerWantsToWait()
    {
        if (participant.displayOpponentStats)
            participant.displayOpponentStats = false;
        participant.currentPawn.EndPawnTurn();
        participant.stage = TacticsParticipantResource.Stage.SelectPawn;
    }

    public void PLayerWantsToSkipTurn()
    {
        if (participant.displayOpponentStats)
            participant.displayOpponentStats = false;
        participant.SkipTurnHandler();
    }

    public void PlayerWantsToAttack()
    {
        participant.stage = TacticsParticipantResource.Stage.DisplayTargets;
    }
}
