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
            tile.CollisionLayer = 1;

            // Generate collision shape
            mesh.CreateTrimeshCollision();
            var generatedBody = mesh.GetChild<StaticBody3D>(0);
            var generatedShape = generatedBody.GetChild<CollisionShape3D>(0);

            // Copy the generated shape into the custom tile body.
            var shape = new CollisionShape3D
            {
                Shape = generatedShape.Shape,
                Transform = generatedShape.Transform,
            };
            tile.AddChild(shape);

            // Positioning
            tile.Position = mesh.Position;
            mesh.Position = Vector3.Zero;

            // Re-parent mesh under tile
            tiles.RemoveChild(mesh);
            mesh.Name = "Tile";
            tile.AddChild(mesh);

            // Configure tile
            tile.ConfigureTile();
            tile.SetProcess(true);

            // Add tile to scene
            tiles.AddChild(tile);

            // Cleanup
            generatedShape.QueueFree();
            generatedBody.QueueFree();
        }
    }
}
