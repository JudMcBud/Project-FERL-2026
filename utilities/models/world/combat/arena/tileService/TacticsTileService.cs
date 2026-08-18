using System.Runtime.Serialization.Formatters;
using Godot;

public partial class TacticsTileService : Node3D
{
    public const string TileSource =
        "res://utilities/modules/tactics/level/arena/tile/TacticsTile.cs";

    public void TilesIntoStaticbodies(Node3D tiles)
    {
        foreach (MeshInstance3D mesh in tiles.GetChildren())
        {
            // Create custom tile instead of using CreateTrimeshCollision's StaticBody3D
            TacticsTile tile = new TacticsTile();
            tile.Name = "Tile";

            // Generate collision shape
            mesh.CreateTrimeshCollision();
            var generatedBody = mesh.GetChild<StaticBody3D>(0);
            var shape = generatedBody.GetChild<CollisionShape3D>(0);

            // Move the shape into custom tile
            generatedBody.RemoveChild(shape);
            tile.AddChild(shape);

            // Positioning
            tile.Position = mesh.Position;
            mesh.Position = Vector3.Zero;

            // Re-parent mesh under tile
            tiles.RemoveChild(mesh);
            tile.AddChild(mesh);

            // Configure tile
            tile.ConfigureTile();
            tile.SetProcess(true);

            // Add tile to scene
            tiles.AddChild(tile);

            // Cleanup
            generatedBody.QueueFree();
        }
    }
}
