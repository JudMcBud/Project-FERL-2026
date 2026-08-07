using System.Collections.Generic;
using System.Linq;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;
using Godot.Collections;

public partial class TacticsArenaService : RefCounted
{
    public static readonly TacticsTileService TileService = ResourceLoader.Load<TacticsTileService>(
        "res://utilities/models/world/combat/arena/tileService/TacticsTileService.cs"
    );

    public TacticsArenaResource resource;

    public TacticsArenaService(TacticsArenaResource _resource)
    {
        resource = _resource;
    }

    public void Setup(TacticsArena arena)
    {
        if (resource == null)
        {
            GD.PushError(
                "TacticsArena needs an ArenaResource from /data/models/world/combat/arena/"
            );
        }
        else
        {
            resource.ResetAllTileMarkers += arena.ResetAllTileMarkers;
            resource.GetPathfindingTileStack += (to) => arena.GetPathfindingTileStack(to);
            resource.MarkHoverTile += arena.MarkHoverTile;
        }
    }

    public void ResetAllTileMarkers(TacticsArena arena)
    {
        foreach (TacticsTile _t in arena.GetNode("Tiles").GetChildren())
        {
            _t.ResetMarkers();
        }
    }

    public void ConfigureTiles(TacticsArena arena)
    {
        arena.GetNode<Node3D>("Tiles").Visible = true;
        Node3D _tiles = arena.GetNode<Node3D>("Tiles");
        TileService.TilesIntoStaticbodies(_tiles);
    }

    public void ProcessSurroundingTiles(TacticsTile rootTile, float height, Array<Node> alliesOnMap)
    {
        List<TacticsTile> _tilesProcessQueue = [rootTile];

        while (_tilesProcessQueue.Count() != 0)
        {
            TacticsTile _currentTile = _tilesProcessQueue[0];
            _tilesProcessQueue.RemoveAt(0);

            void _AddTilesToTilesList(TacticsTile _neighbor)
            {
                _neighbor.pfRoot = _currentTile;
                _neighbor.pfDistance = _currentTile.pfDistance + 1;
                _tilesProcessQueue.Add(_neighbor);
            }

            foreach (TacticsTile _neighbor in _currentTile.GetNeighbors(height))
            {
                if (_neighbor.pfRoot == null && _neighbor != rootTile)
                {
                    if (!_neighbor.IsTaken())
                    {
                        _AddTilesToTilesList(_neighbor);
                    }
                    else if (alliesOnMap.Count() <= 0)
                    {
                        if (alliesOnMap.Contains(_neighbor.GetTileOccupier()))
                        {
                            _AddTilesToTilesList(_neighbor);
                        }
                    }
                }
            }
        }
    }

    public List<TacticsTile> GetPathfindingTileStack(TacticsTile to)
    {
        List<TacticsTile> pathTilesStack = [];
        while (to != null)
        {
            to.hover = true;
            pathTilesStack.Insert(0, to);
            to = to.pfRoot;
        }
        resource.pathTilesStack = pathTilesStack;
        return pathTilesStack;
    }

    public TacticsTile GetNearestTargetAdjacentTile(TacticsPawn pawn, Array<Node> targetPawns)
    {
        TacticsTile _nearestTarget = null;

        foreach (TacticsPawn _p in targetPawns)
        {
            if (_p.stats.currentHealth <= 0)
                continue;
            foreach (TacticsTile _n in _p.GetTile().GetNeighbors(pawn.stats.jump))
            {
                if (_nearestTarget == null || _n.pfDistance < _nearestTarget.pfDistance)
                {
                    if (_n.pfDistance > 0 && !_n.IsTaken())
                    {
                        _nearestTarget = _n;
                    }
                }
            }
        }

        while (_nearestTarget != null && !_nearestTarget.reachable)
        {
            _nearestTarget = _nearestTarget.pfRoot;
        }

        if (_nearestTarget != null)
        {
            return _nearestTarget;
        }
        else
        {
            return pawn.GetTile();
        }
    }

    public TacticsPawn GetWeakestAttackablePawn(Array<Node> pawnList)
    {
        TacticsPawn weakest = null;

        foreach (TacticsPawn _p in pawnList)
        {
            if (weakest == null || _p.stats.currentHealth < weakest.stats.currentHealth)
            {
                if (_p.stats.currentHealth > 0 && _p.GetTile().attackable)
                {
                    weakest = _p;
                }
            }
        }
        return weakest;
    }

    public void MarkHoverTile(TacticsArena arena, TacticsTile tile)
    {
        foreach (TacticsTile _t in arena.GetNode("Tiles").GetChildren())
        {
            _t.hover = false;
        }
        if (tile != null)
        {
            tile.hover = true;
        }
    }

    public void MarkReachableTiles(TacticsArena arena, TacticsTile root, float distance)
    {
        foreach (TacticsTile _t in arena.GetNode("Tiles").GetChildren())
        {
            bool _hasDist = _t.pfDistance > 0;
            bool _reachable = _t.pfDistance <= distance;
            bool _notTaken = !_t.IsTaken();
            bool _isRoot = _t == root;

            _t.reachable = (_hasDist && _reachable && _notTaken) || _isRoot;
        }
    }

    public void MarkAttackableTiles(TacticsArena arena, TacticsTile root, float distance)
    {
        foreach (TacticsTile _t in arena.GetNode("TIles").GetChildren())
        {
            bool _hasDist = _t.pfDistance > 0;
            bool _reachable = _t.pfDistance <= distance;
            bool _isRoot = _t == root;

            _t.attackable = (_hasDist && _reachable) || _isRoot;
        }
    }
}
