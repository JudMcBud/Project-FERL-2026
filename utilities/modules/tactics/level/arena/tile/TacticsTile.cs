using System;
using System.Collections.Generic;
using Godot;

public partial class TacticsTile : StaticBody3D
{
    public bool reachable = false;
    public bool attackable = false;
    public bool hover = false;
    public TacticsTile pfRoot;
    public float pfDistance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void ResetMarker() { }

    public List<TacticsTile> GetNeighbors(float height)
    {
        List<TacticsTile> r = [];
        return r;
    }

    public bool IsTaken()
    {
        return false;
    }

    public Object GetTileOccupier()
    {
        return new Object();
    }
}
