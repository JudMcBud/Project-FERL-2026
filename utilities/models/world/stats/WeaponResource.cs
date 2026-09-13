using Godot;
using Godot.Collections;

public enum WeaponType
{
    Sword,
    Lance,
    Axe,
    Bow,
    Tome,
    Gauntlet,
    Dagger,
}

public enum DamageType
{
    Physical,
    Magical,
    Dynamic,
}

[GlobalClass]
public partial class WeaponResource : Resource
{
    [Export]
    public string id = "";

    [Export]
    public string displayName = "";

    [Export]
    public StatSet weaponBonuses = new StatSet();

    [Export]
    public Array<int> attackRanges = new Array<int> { 1 };

    [Export]
    public WeaponType weaponType;

    [Export]
    public DamageType damageType;

    [Export]
    public Array<Resource> passiveEffects = new Array<Resource>();

    [Export]
    public Array<Resource> activeEffects = new Array<Resource>();

    public Dictionary ToDictionary() =>
        new Dictionary
        {
            [nameof(id)] = id,
            [nameof(displayName)] = displayName,
            [nameof(weaponType)] = weaponType.ToString(),
            [nameof(damageType)] = damageType.ToString(),
        };
}
