using System;
using Godot;
using Godot.Collections;

public partial class Stats : Node
{
    public Dictionary modifiers = new Dictionary();
    public string overrideName;
    public string expertise;
    public int level = 1;

    #region Base Stats
    public int movement;
    public int jump;
    public int maxHealth;
    public int currentHealth;
    public string sprite;
    #endregion
    #region Offensive Stats
    public int attackPower;
    public int attackRange;
    #endregion

    public void ImportStats(StatsResource stats)
    {
        overrideName = stats.overrideName;
        expertise = stats.expertise;
        level = stats.level;
        movement = stats.movement;
        jump = stats.jump;
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
        sprite = stats.sprite;
        attackPower = stats.attackPower;
        attackRange = stats.attackRange;
    }

    public void ApplyToCurrentHealth(int changeinHealth)
    {
        currentHealth = Math.Clamp(currentHealth + changeinHealth, 0, maxHealth);
    }
}
