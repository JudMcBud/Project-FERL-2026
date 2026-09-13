using Godot;
using Godot.Collections;

public enum SkillType
{
    Offensive,
    Defensive,
    Support,
}

public enum SkillSlot
{
    Offensive,
    Defensive,
    Support,
    Universal,
}

[GlobalClass]
public partial class SkillResource : Resource
{
    [Export]
    public string id = "";

    [Export]
    public string displayName = "";

    [Export]
    public SkillType skillType;

    [Export]
    public EffectResource effect;

    [Export]
    public int cooldownTurns;

    [Export]
    public bool isPassive;

    public Dictionary ToDictionary() =>
        new Dictionary
        {
            [nameof(id)] = id,
            [nameof(displayName)] = displayName,
            [nameof(skillType)] = skillType.ToString(),
            [nameof(cooldownTurns)] = cooldownTurns,
            [nameof(isPassive)] = isPassive,
        };
}
