using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;
using Godot.Collections;

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
        Array<Node> alliesOnMap = null
    )
    {
        alliesOnMap ??= [];
        service.ProcessSurroundingTiles(rootTile, height, alliesOnMap);
    }

    public List<TacticsTile> GetPathfindingTileStack(TacticsTile to)
    {
        return service.GetPathfindingTileStack(to);
    }

    public TacticsTile GetNearestTargetAdjacentTile(TacticsPawn pawn, Array<Node> targetPawns)
    {
        return service.GetNearestTargetAdjacentTile(pawn, targetPawns);
    }

    public TacticsPawn GetWeakestAttackablePawn(Array<Node> pawnList)
    {
        return service.GetWeakestAttackablePawn(pawnList);
    }

    public void MarkHoverTile(TacticsTile tile)
    {
        service.MarkHoverTile(this, tile);
    }

    public void MarkReachableTiles(TacticsTile root, float distance)
    {
        service.MarkReachableTiles(this, root, distance);
    }

    public void MarkAttackableTiles(TacticsTile root, float distance)
    {
        service.MarkAttackableTiles(this, root, distance);
    }
}
