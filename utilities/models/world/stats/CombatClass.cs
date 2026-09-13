using Godot;
using Godot.Collections;

[GlobalClass]
public partial class CombatClass : Resource
{
    [Export]
    public string id = "";

    [Export]
    public string displayName = "";

    [Export]
    public StatSet classStats = new StatSet();

    [Export]
    public Array<Resource> passiveEffects = new Array<Resource>();

    [Export]
    public Array<Resource> activeEffects = new Array<Resource>();

    public virtual void OnEquip(Stats owner) { }

    public virtual void OnUnequip(Stats owner) { }

    public virtual void OnCombatStart(Stats owner, Stats foe) { }

    public virtual void OnTurnStart(Stats owner) { }

    public Dictionary ToDictionary() =>
        new Dictionary
        {
            [nameof(id)] = id,
            [nameof(displayName)] = displayName,
            [nameof(classStats)] = classStats?.ToDictionary(),
        };
}
