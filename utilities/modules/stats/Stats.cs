using System;
using Godot;

public partial class Stats : Node
{
    #region Base Stats
    public int jump;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    #endregion

    public void ApplyToCurrentHealth(int changeinHealth)
    {
        currentHealth = Math.Clamp(currentHealth + changeinHealth, 0, maxHealth);
    }
}
