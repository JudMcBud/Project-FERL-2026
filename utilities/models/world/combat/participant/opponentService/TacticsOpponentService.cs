using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;
using Godot.Collections;

public partial class TacticsOpponentService : RefCounted
{
    TacticsParticipantResource resource;
    TacticsCameraResource camera;
    TacticsControlsResource controls;
    TacticsArena arena;

    public TacticsOpponentService(
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

    public bool IsPawnConfigured(TacticsOpponent opponent)
    {
        foreach (TacticsPawn pawn in opponent.GetChildren())
        {
            if (!pawn.Center())
                return false;
        }
        return true;
    }

    public void ChoosePawn(TacticsOpponent opponent)
    {
        arena.ResetAllTileMarkers();
        foreach (TacticsPawn p in opponent.GetChildren())
        {
            if (p.CanAct() && p.IsAlive())
            {
                resource.currentPawn = p;
                resource.stage = TacticsParticipantResource.Stage.ShowActions;
                return;
            }
        }
    }

    public void ChaseNearestEnemy(TacticsOpponent opponent, Node playerNode)
    {
        if (resource.currentPawn.resource.canMove)
        {
            arena.ResetAllTileMarkers();
            arena.ProcessSurroundingTiles(
                resource.currentPawn.GetTile(),
                resource.currentPawn.stats.movement,
                opponent.GetChildren()
            );
            arena.MarkReachableTiles(
                resource.currentPawn.GetTile(),
                resource.currentPawn.stats.movement
            );

            TacticsTile to = arena.GetNearestTargetAdjacentTile(
                resource.currentPawn,
                playerNode.GetChildren()
            );
            resource.currentPawn.resource.pathfindingTilestack = arena.GetPathfindingTileStack(to);
            camera.target = to;
            resource.stage = TacticsParticipantResource.Stage.ShowMovements;
        }
        else
        {
            resource.stage = TacticsParticipantResource.Stage.SelectPawn;
        }
    }

    public void IsPawnDoneMoving()
    {
        if (resource.currentPawn.resource.pathfindingTilestack.Count == 0)
        {
            resource.stage = TacticsParticipantResource.Stage.SelectLocation;
        }
    }

    public void ChoosePawnToAttack()
    {
        arena.ResetAllTileMarkers();
        arena.ProcessSurroundingTiles(
            resource.currentPawn.GetTile(),
            resource.currentPawn.stats.attackRange
        );
        arena.MarkAttackableTiles(
            resource.currentPawn.GetTile(),
            resource.currentPawn.stats.attackRange
        );

        resource.attackablePawn = arena.GetWeakestAttackablePawn(resource.targets.GetChildren());
        if (resource.attackablePawn != null)
        {
            controls.SetActionsMenuVisibilityHandler(true, resource.attackablePawn);
            camera.target = resource.attackablePawn;
        }

        resource.stage = TacticsParticipantResource.Stage.MovePawn;
    }
}
