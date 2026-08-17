using System;
using System.Collections.Generic;
using Godot;

public partial class TacticsTileRaycast : Node3D
{
    #region --- Methods ---
    public List<TacticsTile> GetAllNeighbors(float height)
    {
        List<TacticsTile> neighbors = [];
        TacticsTile parentTile = GetParent<TacticsTile>();

        foreach (RayCast3D ray in GetNode("Neighbors").GetChildren())
        {
            TacticsTile obj = ray.GetCollider() as TacticsTile;

            if (obj == null || obj == parentTile)
                continue;

            if (
                Math.Abs(obj.GlobalPosition.Y - parentTile.GlobalPosition.Y) <= height
                && !neighbors.Contains(obj)
            )
            {
                neighbors.Add(obj);
            }
        }
        return neighbors;
    }

    public Object GetObjectAbove()
    {
        return GetNode<RayCast3D>("Above").GetCollider();
    }
    #endregion
}
