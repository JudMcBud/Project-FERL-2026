using Godot;
using Godot.Collections;

[GlobalClass]
public partial class EffectTriggerCondition : Resource
{
    [Export]
    public bool manualActivation;

    [Export]
    public float ownerHpThreshold = -1;

    [Export]
    public float foeHpThreshold = -1;

    [Export]
    public StatType thresholdStat;

    [Export]
    public float ownerStatThreshold = -1;

    [Export]
    public float foeStatThreshold = -1;

    [Export]
    public int turnOrderPosition;

    [Export]
    public bool combatInitiator;

    public virtual bool Evaluate(Stats owner, Stats foe)
    {
        if (owner == null)
            return false;
        if (ownerHpThreshold >= 0 && owner.currentHealth > owner.maxHealth * ownerHpThreshold)
            return false;
        if (
            foeHpThreshold >= 0
            && (foe == null || foe.currentHealth > foe.maxHealth * foeHpThreshold)
        )
            return false;
        if (ownerStatThreshold >= 0 && owner.GetEffectiveStat(thresholdStat) < ownerStatThreshold)
            return false;
        if (
            foeStatThreshold >= 0
            && (foe == null || foe.GetEffectiveStat(thresholdStat) < foeStatThreshold)
        )
            return false;
        return true;
    }
}

[GlobalClass]
public partial class EffectResult : Resource
{
    [Export]
    public Array<StatModifier> modifiers = new Array<StatModifier>();

    [Export]
    public int damage;

    [Export]
    public int healing;

    [Export]
    public int extraActions;

    [Export]
    public string statusEffectId = "";

    public virtual void Execute(Stats owner, Stats foe)
    {
        if (owner == null)
            return;
        foreach (StatModifier modifier in modifiers)
            owner.ApplyModifier(modifier);
        owner.ApplyToCurrentHealth(healing);
        foe?.ApplyToCurrentHealth(-damage);
    }
}

[GlobalClass]
public partial class EffectResource : Resource
{
    [Export]
    public string id = "";

    [Export]
    public string displayName = "";

    [Export]
    public bool isPassive;

    [Export]
    public bool isManualActivationAllowed = true;

    [Export]
    public EffectTriggerCondition triggerCondition;

    [Export]
    public EffectResult result;

    public virtual bool CanActivate(Stats owner, Stats foe) =>
        isManualActivationAllowed && (triggerCondition?.Evaluate(owner, foe) ?? true);

    public virtual void Execute(Stats owner, Stats foe) => result?.Execute(owner, foe);

    public Dictionary ToDictionary() =>
        new Dictionary
        {
            [nameof(id)] = id,
            [nameof(displayName)] = displayName,
            [nameof(isPassive)] = isPassive,
            [nameof(isManualActivationAllowed)] = isManualActivationAllowed,
        };
}
