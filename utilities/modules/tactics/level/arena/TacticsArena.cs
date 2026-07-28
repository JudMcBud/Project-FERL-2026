using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsArena : Node3D
{
    [Export]
    public TacticsArenaResource resource;

    public TacticsArenaService service;

    public override void _Ready()
    {
        resource = GD.Load<TacticsArenaResource>(
            "res://utilities/models/world/combat/arena/tacticsArenaResource.tres"
        );
        service = new TacticsArenaService(resource);
        service.Setup(this);
    }

    public void ResetAllTileMarkers()
    {
        service.ResetAllTileMarkers(this);
    }

    public void ConfigureTiles()
    {
        service.ConfigureTiles(this);
    }

    public void ProcessSurroundingTiles(
        TacticsTile rootTile,
        float height,
        List<Node3D> alliesOnMap = null
    )
    {
        alliesOnMap ??= [];
        service.ProcessSurroundingTiles(rootTile, height, alliesOnMap);
    }

    public List<TacticsTile> GetPathfindingTileStack(TacticsTile to)
    {
        return service.GetPathfindingTileStack(to);
    }

    public TacticsTile GetNearestTargetAdjacentTile(TacticsPawn pawn, List<TacticsPawn> targetPawns)
    {
        return service.GetNearestTargetAdjacentTile(pawn, targetPawns);
    }

    public TacticsPawn GetWeakestAttackablePawn(List<TacticsPawn> pawnList)
    {
        return service.GetWeakestAttackablePawn(pawnList);
    }

    public void MarkHoverTile(TacticsTile tile)
    {
        service.MarkHoverTile(tile);
    }

    public void MarkReachableTiles(TacticsTile root)
    {
        service.MarkReachableTiles(root);
    }

    public void MarkAttackableTiles(TacticsTile root, float distance)
    {
        service.MarkAttackableTiles(this, root, distance);
    }
}
