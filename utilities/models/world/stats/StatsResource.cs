using System;
using Godot;
using Godot.Collections;

[GlobalClass]
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

    [Export(PropertyHint.GlobalFile, "*.png")]
    public string sprite = "";
    #endregion

    #region Base Stats
    [ExportCategory("Base")]
    [Export]
    public StatSet baseStats = new StatSet();

    [Export]
    public int movement = 3;

    [Export]
    public int jump = 1;

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

    public StatSet GetBaseStats()
    {
        StatSet result = baseStats?.Duplicate() ?? new StatSet();
        StatSet defaults = new StatSet();

        if (result.healthPoints == defaults.healthPoints && maxHealth != 5)
            result.healthPoints = maxHealth;
        if (result.strength == defaults.strength && attackPower != 1)
            result.strength = attackPower;
        if (result.speed == defaults.speed && movement != 3)
            result.speed = movement;

        return result;
    }

    public Dictionary ToDictionary() =>
        new Dictionary
        {
            [nameof(overrideName)] = overrideName,
            [nameof(expertise)] = expertise,
            [nameof(baseStats)] = GetBaseStats().ToDictionary(),
            [nameof(sprite)] = sprite,
        };

    // #region Methods
    // public void SetJump()
    // {
    //     jump = (int)Math.Floor(jump / 2.0);
    // }
    // #endregion
}
