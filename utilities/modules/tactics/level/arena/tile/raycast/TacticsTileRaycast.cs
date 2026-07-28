using System;
using System.Collections.Generic;
using Godot;

public partial class TacticsTileRaycast : Node3D
{
    #region --- Methods ---
    public List<TacticsTile> GetAllNeighbors(float height)
    {
        List<TacticsTile> neighbors = [];
        foreach (RayCast3D ray in GetNode("Neighbors").GetChildren())
        {
            TacticsTile obj = (TacticsTile)ray.GetCollider();

            if (
                obj != null
                && Math.Abs(obj.GlobalPosition.Y - GetParent<Node3D>().GlobalPosition.Y) <= height
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
