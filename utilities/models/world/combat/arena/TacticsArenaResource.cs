using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class TacticsArenaResource : Resource
{
    [Signal]
    public delegate void ResetAllTileMarkersEventHandler();

    [Signal]
    public delegate void GetPathfindingTileStackEventHandler(TacticsTile tile);

    [Signal]
    public delegate void MarkHoverTileEventHandler(TacticsTile tile);

    public List<TacticsTile> pathTilesStack = [];

    public void OnResetAllTileMarkers()
    {
        EmitSignal(SignalName.ResetAllTileMarkers);
    }

    public List<TacticsTile> OnGetPathfindingTileStack(TacticsTile tile)
    {
        EmitSignal(SignalName.GetPathfindingTileStack, tile);
        return pathTilesStack;
    }

    public void OnMarkHoverTileEventHandler(TacticsTile tile)
    {
        EmitSignal(SignalName.MarkHoverTile, tile);
    }
}
