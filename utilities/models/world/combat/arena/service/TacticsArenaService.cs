using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsArenaService : RefCounted
{
    public TacticsArenaService(TacticsArenaResource resource) { }

    public void Setup(TacticsArena arena) { }

    public void ResetAllTileMarkers(TacticsArena arena) { }

    public void ConfigureTiles(TacticsArena arena) { }

    public void ProcessSurroundingTiles(
        TacticsTile rootTile,
        float height,
        List<Node3D> alliesOnMap
    ) { }

    public List<TacticsTile> GetPathfindingTileStack(TacticsTile to)
    {
        List<TacticsTile> pathTilesStack = [];
        return pathTilesStack;
    }

    public TacticsTile GetNearestTargetAdjacentTile(TacticsPawn pawn, List<TacticsPawn> targetPawns)
    {
        TacticsTile t = null;
        return t;
    }

    public TacticsPawn GetWeakestAttackablePawn(List<TacticsPawn> pawnList)
    {
        TacticsPawn p = null;
        return p;
    }

    public void MarkHoveTile(TacticsTile tile) { }

    public void MarkReachableTiles(TacticsTile root) { }

    public void MarkAttackableTiles(TacticsArena arena, TacticsTile root, float distance) { }
}
