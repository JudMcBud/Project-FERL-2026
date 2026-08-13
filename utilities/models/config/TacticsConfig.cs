using System;
using System.Collections.Generic;
using Godot;

namespace Game.Models.Config.TacticsConfig;

public partial class TacticsConfig : Node3D
{
    #region --- Props ---
    private static Dictionary<string, string> color = new Dictionary<string, string>
    {
        { "white", "FFFFFF3F" },
        { "blueCola", "008fdbBF" },
        { "blueBolt", "0aa9ffBF" },
        { "rossoCorsa", "d10000BF" },
        { "coralRed", "ff4242BF" },
    };
    public static Dictionary<string, StandardMaterial3D> matColor = new Dictionary<
        string,
        StandardMaterial3D
    >
    {
        { "hover", CreateMaterial(color["white"]) },
        { "reachable", CreateMaterial(color["blueCola"]) },
        { "hoverReachable", CreateMaterial(color["blueBolt"]) },
        { "attackable", CreateMaterial(color["rossoCorsa"]) },
        { "hoverAttackable", CreateMaterial(color["coralRed"]) },
    };

    public static Dictionary<string, int> pawn = new Dictionary<string, int>
    {
        { "baseWalkSpeed", 8 },
        { "animationFrame", 1 },
        { "minHeightToJump", 1 },
        { "gravityStrength", 6 },
        { "minTimeForAttack", 1 },
    };

    public static Dictionary<string, int> view = new Dictionary<string, int>
    {
        { "defaultTCamZoom", 30 },
    };

    public static readonly List<string> uiElem = ["%Actions", "%Hints"];
    #endregion

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
