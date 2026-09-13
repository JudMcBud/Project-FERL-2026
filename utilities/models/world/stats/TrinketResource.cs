using Godot;
using Godot.Collections;

[GlobalClass]
public partial class TrinketResource : Resource
{
    [Export]
    public string id = "";

    [Export]
    public string displayName = "";

    [Export]
    public StatSet trinketBonuses = new StatSet();

    [Export]
    public Array<string> allowedClassIds = new Array<string>();

    [Export]
    public Array<Resource> passiveEffects = new Array<Resource>();

    [Export]
    public Array<Resource> activeEffects = new Array<Resource>();

    public Dictionary ToDictionary() =>
        new Dictionary
        {
            [nameof(id)] = id,
            [nameof(displayName)] = displayName,
            [nameof(allowedClassIds)] = allowedClassIds,
        };
}
