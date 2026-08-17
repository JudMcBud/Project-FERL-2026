using System.Runtime.Serialization.Formatters;
using Godot;

public partial class TacticsTileService : Node3D
{
    public const string TileSource =
        "res://utilities/modules/tactics/level/arena/tile/TacticsTile.cs";

    public void TilesIntoStaticbodies(Node3D tiles)
    {
        foreach (MeshInstance3D _t in tiles.GetChildren())
        {
            _t.CreateTrimeshCollision();
            TacticsTile _staticBody = (TacticsTile)_t.GetChild<StaticBody3D>(0);
            _staticBody.Position = _t.Position;

            _t.Position = Vector3.Zero;
            _t.Name = "Tile";
            _t.RemoveChild(_staticBody);
            tiles.RemoveChild(_t);
            _staticBody.AddChild(_t);
            _staticBody.SetScript(GD.Load<Script>(TileSource));

            _staticBody.ConfigureTile();

            _staticBody.SetProcess(true);

            tiles.AddChild(_staticBody);
        }
    }
}
