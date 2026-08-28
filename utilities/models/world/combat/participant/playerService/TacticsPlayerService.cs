using System;
using System.Collections.Generic;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;
using Godot.Collections;

public partial class TacticsPlayerService : RefCounted
{
    public TacticsParticipantResource resource;
    public TacticsCameraResource camera;
    public TacticsControlsResource controls;
    public TacticsArena arena;

    public TacticsPlayerService(
        TacticsParticipantResource _resource,
        TacticsCameraResource _camera,
        TacticsControlsResource _controls,
        TacticsArena _arena
    )
    {
        resource = _resource;
        camera = _camera;
        controls = _controls;
        arena = _arena;
    }

    public void ToggleEnemyStats(Node opponentNode)
    {
        Array<Node> enemyPawns = opponentNode.GetChildren();

        if (resource.displayOpponentStats)
        {
            foreach (TacticsPawn _pawn in enemyPawns)
            {
                _pawn.resource.pawnHudEnabled = true;
                _pawn.ShowPawnStats(true);
            }
        }
        else
        {
            foreach (TacticsPawn _pawn in enemyPawns)
            {
                if (_pawn.resource.pawnHudEnabled == true)
                {
                    _pawn.ShowPawnStats(false);
                    _pawn.resource.pawnHudEnabled = false;
                }
            }
        }
    }

    public bool IsPawnConfigured(TacticsPlayer player)
    {
        foreach (TacticsPawn _pawn in player.GetChildren())
        {
            if (_pawn is TacticsPawn)
            {
                if (!_pawn.Center())
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ShowAvailablePawnActions()
    {
        controls.SetActionsMenuVisibilityHandler(true, resource.currentPawn);
        arena.ResetAllTileMarkers();
        arena.MarkHoverTile(resource.currentPawn.GetTile());
    }

    public void ShowAvailableMovements()
    {
        arena.ResetAllTileMarkers();

        TacticsPawn p = resource.currentPawn;
        if (p == null)
            return;

        camera.target = p;
        arena.ProcessSurroundingTiles(p.GetTile(), p.stats.movement);
        arena.MarkReachableTiles(p.GetTile(), p.stats.movement);
        resource.stage = TacticsParticipantResource.Stage.SelectLocation;
    }

    public void DisplayAttackableTargets()
    {
        arena.ResetAllTileMarkers();
        TacticsPawn p = resource.currentPawn;
        if (p == null)
            return;
        resource.displayOpponentStats = true;

        camera.target = p;
        Array<Node> targetPawns = resource.targets?.GetChildren() ?? [];
        arena.ProcessSurroundingTiles(p.GetTile(), p.stats.attackRange, targetPawns);
        arena.MarkAttackableTiles(p.GetTile(), p.stats.attackRange, targetPawns);
        resource.stage = TacticsParticipantResource.Stage.SelectAttackTarget;
    }

    public void MovePawn()
    {
        TacticsPawn p = resource.currentPawn;
        controls.SetActionsMenuVisibilityHandler(false, p);
        if (p.resource.pathfindingTilestack.Count == 0)
        {
            resource.stage = p.CanAct()
                ? TacticsParticipantResource.Stage.SelectPawn
                : TacticsParticipantResource.Stage.ShowActions;
        }
    }
}
