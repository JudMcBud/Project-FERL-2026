using System;
using Godot;
using Godot.Collections;

namespace Game.Models.Config.TacticsConfig;

public partial class TacticsConfig : Node3D
{
    public static Dictionary<string, StandardMaterial3D> matColor = new Dictionary<
        string,
        StandardMaterial3D
    >
    {
        { "hover", CreateMaterial(Convert.ToString(Colors.White)) },
        { "reachable", CreateMaterial(Convert.ToString(Colors.MidnightBlue)) },
        { "hoverReachable", CreateMaterial(Convert.ToString(Colors.LightSkyBlue)) },
        { "attackable", CreateMaterial(Convert.ToString(Colors.DarkRed)) },
        { "hoverAttackable", CreateMaterial(Convert.ToString(Colors.Tomato)) },
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    private static StandardMaterial3D CreateMaterial(
        Variant colorHex,
        Texture2D texture = null,
        BaseMaterial3D.ShadingModeEnum shadedMode = BaseMaterial3D.ShadingModeEnum.PerPixel
    )
    {
        StandardMaterial3D material = new StandardMaterial3D();
        material.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
        material.AlbedoColor = Color.FromString(Convert.ToString(colorHex), Colors.Fuchsia);
        material.AlbedoTexture = texture;
        material.ShadingMode = shadedMode;
        return material;
    }
}
