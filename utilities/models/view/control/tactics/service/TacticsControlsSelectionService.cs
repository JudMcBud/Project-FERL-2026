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

        if (Input.IsActionPressed("ui_accept") && ctrl.currentPawn.CanAct())
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

    private static TacticsTile ResolveTileFromHit(Object hit)
    {
        if (hit is TacticsTile tile)
            return tile;
        if (hit is TacticsPawn pawn)
            return pawn.GetTile();
        return null;
    }

    public TacticsPawn SelectHoveredPawn(TacticsControls ctrl)
    {
        TacticsPawn pawn = inputService.Get3DCanvasMousePosition(2, ctrl) as TacticsPawn;
        TacticsTile tile =
            pawn != null
                ? pawn.GetTile()
                : ResolveTileFromHit(inputService.Get3DCanvasMousePosition(1, ctrl));
        arena.OnMarkHoverTile(tile);
        if (pawn != null)
            return pawn;
        else if (tile != null)
            return tile.GetTileOccupier() as TacticsPawn;
        else
            return null;
    }

    public TacticsTile SelectHoveredTile(TacticsControls ctrl)
    {
        TacticsPawn pawn = inputService.Get3DCanvasMousePosition(2, ctrl) as TacticsPawn;
        TacticsTile tile =
            pawn != null
                ? pawn.GetTile()
                : ResolveTileFromHit(inputService.Get3DCanvasMousePosition(1, ctrl));
        arena.OnMarkHoverTile(tile);
        return tile;
    }

    public void SelectNewLocation(TacticsControls ctrl)
    {
        TacticsTile tile = ResolveTileFromHit(inputService.Get3DCanvasMousePosition(1, ctrl));
        arena.OnMarkHoverTile(tile);
        if (Input.IsActionPressed("ui_accept") && tile != null && tile.reachable)
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
        participant.attackablePawn = tile != null ? tile.GetTileOccupier() as TacticsPawn : null;
        if (participant.attackablePawn != null)
        {
            controls.SetActionsMenuVisibilityHandler(true, participant.attackablePawn);
            participant.attackablePawn.ShowPawnStats(true);
        }
        if (Input.IsActionJustPressed("ui_accept") && tile != null && tile.attackable)
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

    public void PlayerWantsToSkipTurn()
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
