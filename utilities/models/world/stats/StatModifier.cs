using Godot;

public enum ModifierType
{
    Additive,
    Multiplicative,
    Override,
}

[GlobalClass]
public partial class StatModifier : Resource
{
    [Export]
    public string id = "";

    [Export]
    public StatType stat;

    [Export]
    public float value;

    [Export]
    public ModifierType type = ModifierType.Additive;

    [Export]
    public int durationTurns;

    [Export]
    public string sourceId = "";

    public StatModifier() { }

    public StatModifier(
        string id,
        StatType stat,
        float value,
        ModifierType type = ModifierType.Additive,
        int durationTurns = 0,
        string sourceId = ""
    )
    {
        this.id = id;
        this.stat = stat;
        this.value = value;
        this.type = type;
        this.durationTurns = durationTurns;
        this.sourceId = sourceId;
    }
}
