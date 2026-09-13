using Godot;
using Godot.Collections;

[GlobalClass]
public partial class StatSet : Resource
{
    [Export]
    public float healthPoints = 5;

    [Export]
    public float strength = 1;

    [Export]
    public float magic = 0;

    [Export]
    public float speed = 3;

    [Export]
    public float defense = 0;

    [Export]
    public float resistance = 0;

    [Export]
    public float skill = 0;

    [Export]
    public float luck = 0;

    public float Get(StatType stat)
    {
        return stat switch
        {
            StatType.HealthPoints => healthPoints,
            StatType.Strength => strength,
            StatType.Magic => magic,
            StatType.Speed => speed,
            StatType.Defense => defense,
            StatType.Resistance => resistance,
            StatType.Skill => skill,
            StatType.Luck => luck,
            _ => 0,
        };
    }

    public void Set(StatType stat, float value)
    {
        switch (stat)
        {
            case StatType.HealthPoints:
                healthPoints = value;
                break;
            case StatType.Strength:
                strength = value;
                break;
            case StatType.Magic:
                magic = value;
                break;
            case StatType.Speed:
                speed = value;
                break;
            case StatType.Defense:
                defense = value;
                break;
            case StatType.Resistance:
                resistance = value;
                break;
            case StatType.Skill:
                skill = value;
                break;
            case StatType.Luck:
                luck = value;
                break;
        }
    }

    public StatSet Duplicate() =>
        new StatSet
        {
            healthPoints = healthPoints,
            strength = strength,
            magic = magic,
            speed = speed,
            defense = defense,
            resistance = resistance,
            skill = skill,
            luck = luck,
        };

    public void Add(StatSet other)
    {
        if (other == null)
            return;
        foreach (StatType stat in System.Enum.GetValues<StatType>())
            Set(stat, Get(stat) + other.Get(stat));
    }

    public Dictionary ToDictionary() =>
        new Dictionary
        {
            [nameof(StatType.HealthPoints)] = healthPoints,
            [nameof(StatType.Strength)] = strength,
            [nameof(StatType.Magic)] = magic,
            [nameof(StatType.Speed)] = speed,
            [nameof(StatType.Defense)] = defense,
            [nameof(StatType.Resistance)] = resistance,
            [nameof(StatType.Skill)] = skill,
            [nameof(StatType.Luck)] = luck,
        };
}
