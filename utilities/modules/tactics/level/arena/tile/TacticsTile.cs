using System;
using System.Collections.Generic;
using Game.Models.Config.TacticsConfig;
using Godot;

public partial class TacticsTile : StaticBody3D
{
    #region --- Props ---
    public PackedScene tileRaycast = GD.Load<PackedScene>(
        "res://utilities/modules/tactics/level/arena/tile/raycast/tileRaycast.tscn"
    );

    public bool reachable = false;
    public bool attackable = false;
    public bool hover = false;

    public TacticsTile pfRoot;
    public float pfDistance;

    public StandardMaterial3D hoverMat = TacticsConfig.matColor["hover"];
    public StandardMaterial3D reachableMat = TacticsConfig.matColor["reachable"];
    public StandardMaterial3D hoverReachableMat = TacticsConfig.matColor["hoverReachable"];
    public StandardMaterial3D attackableMat = TacticsConfig.matColor["attackable"];
    public StandardMaterial3D hoverAttackableMat = TacticsConfig.matColor["hoverAttackable"];

    #endregion
    #region --- Processing ---
    public override void _Process(double delta)
    {
        MeshInstance3D tile = GetNodeOrNull("Tile") as MeshInstance3D;
        if (tile == null)
            return;

        tile.Visible = attackable || reachable || hover;

        if (hover)
        {
            if (reachable)
                tile.MaterialOverride = hoverReachableMat;
            else if (attackable)
                tile.MaterialOverride = hoverAttackableMat;
            else
                tile.MaterialOverride = hoverMat;
        }
        else
        {
            if (reachable)
                tile.MaterialOverride = reachableMat;
            else if (attackable)
                tile.MaterialOverride = attackableMat;
        }
    }
    #endregion

    #region --- Methods ---

    public List<TacticsTile> GetNeighbors(float height)
    {
        return GetNode<TacticsTileRaycast>("RayCasting").GetAllNeighbors(height);
    }

    public Object GetTileOccupier()
    {
        return GetNode<TacticsTileRaycast>("RayCasting").GetObjectAbove();
    }

    public bool IsTaken()
    {
        return GetTileOccupier() != null;
    }

    public void ResetMarkers()
    {
        pfRoot = null;
        pfDistance = 0;
        reachable = false;
        attackable = false;
    }

    public void ConfigureTile()
    {
        hover = false;
        Node instance = tileRaycast.Instantiate();
        AddChild(instance);
        ResetMarkers();
    }

    #endregion
}
