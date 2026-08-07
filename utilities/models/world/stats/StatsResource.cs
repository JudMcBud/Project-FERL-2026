using System;
using Godot;

public partial class StatsResource : Resource
{
    #region Properties
    [Export]
    public string overrideName = "";

    [Export]
    public string expertise = "";

    [ExportCategory("Init")]
    [Export(PropertyHint.Enum, "Tank,Flank,Physical,Distance,Support")]
    public int strategy;

    [Export]
    public int level = 1;

    [Export(PropertyHint.GlobalFile, "*.png")]
    public string sprite = "";
    #endregion

    #region Base Stats
    [ExportCategory("Base")]
    [Export]
    public int movement = 3;

    [Export]
    public float jump = 1.5f;

    [Export]
    public int maxHealth = 5;
    #endregion

    #region Offensive Stats
    [ExportCategory("Offensive")]
    [Export]
    public int attackRange = 1;

    [Export]
    public int attackPower = 1;
    #endregion

    // #region Methods
    // public void SetJump()
    // {
    //     jump = (float)Math.Floor(movement / 2.0);
    // }
    // #endregion
}
