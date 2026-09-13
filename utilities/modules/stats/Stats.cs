using System;
using System.Collections.Generic;
using Godot;

public partial class Stats : Node
{
    public StatSet baseStats = new StatSet();
    public StatSet equipmentBonuses = new StatSet();
    public List<StatModifier> modifiers = new List<StatModifier>();
    public List<EffectResource> attachedEffects = new List<EffectResource>();

    public string overrideName = "";
    public string expertise = "";
    public string sprite = "";
    public int jump = 1;
    public int currentHealth;

    public CombatClass combatClass;
    public WeaponResource weapon;
    public TrinketResource trinket;
    public SkillResource offensiveSkill;
    public SkillResource defensiveSkill;
    public SkillResource supportSkill;
    public SkillResource universalSkill;

    public int movement => (int)GetEffectiveStat(StatType.Speed);
    public int maxHealth => (int)GetEffectiveStat(StatType.HealthPoints);
    public int attackPower => (int)GetEffectiveStat(StatType.Strength);
    public int attackRange => weapon?.attackRanges?.Count > 0 ? weapon.attackRanges[0] : 1;

    public void ImportStats(StatsResource stats)
    {
        if (stats == null)
            return;
        overrideName = stats.overrideName;
        expertise = stats.expertise;
        sprite = stats.sprite;
        jump = stats.jump;
        baseStats = stats.GetBaseStats();
        equipmentBonuses = new StatSet();
        modifiers.Clear();
        attachedEffects.Clear();
        combatClass = null;
        weapon = null;
        trinket = null;
        currentHealth = maxHealth;
    }

    public float GetEffectiveStat(StatType stat)
    {
        float result = baseStats.Get(stat) + equipmentBonuses.Get(stat);
        float multiplier = 1;
        float additive = 0;
        float overrideValue = result;
        bool hasOverride = false;
        foreach (StatModifier modifier in modifiers)
        {
            if (modifier == null || modifier.stat != stat)
                continue;
            switch (modifier.type)
            {
                case ModifierType.Multiplicative:
                    multiplier *= modifier.value;
                    break;
                case ModifierType.Additive:
                    additive += modifier.value;
                    break;
                case ModifierType.Override:
                    overrideValue = modifier.value;
                    hasOverride = true;
                    break;
            }
        }
        result = (result * multiplier) + additive;
        return hasOverride ? overrideValue : result;
    }

    public void ApplyModifier(StatModifier modifier)
    {
        if (modifier == null)
            return;
        RemoveModifier(modifier.id);
        modifiers.Add(modifier);
        RecalculateCurrentHealthAfterMaxChange();
    }

    public void RemoveModifier(string id)
    {
        modifiers.RemoveAll(modifier => modifier != null && modifier.id == id);
        RecalculateCurrentHealthAfterMaxChange();
    }

    // Call once per combat turn from the combat manager.
    public void TickModifiers()
    {
        for (int index = modifiers.Count - 1; index >= 0; index--)
        {
            StatModifier modifier = modifiers[index];
            if (modifier == null || modifier.durationTurns <= 0)
                continue;
            modifier.durationTurns--;
            if (modifier.durationTurns <= 0)
                modifiers.RemoveAt(index);
        }
        RecalculateCurrentHealthAfterMaxChange();
    }

    public void RecalculateCurrentHealthAfterMaxChange() =>
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);

    public void ApplyToCurrentHealth(int changeInHealth) =>
        currentHealth = Math.Clamp(currentHealth + changeInHealth, 0, maxHealth);

    public void EquipClass(CombatClass cls)
    {
        UnequipClass();
        combatClass = cls;
        if (cls == null)
            return;
        equipmentBonuses.Add(cls.classStats);
        AttachEffects(cls.passiveEffects);
        AttachEffects(cls.activeEffects);
        cls.OnEquip(this);
        RecalculateCurrentHealthAfterMaxChange();
    }

    public void UnequipClass()
    {
        if (combatClass == null)
            return;
        equipmentBonuses = Subtract(equipmentBonuses, combatClass.classStats);
        DetachEffects(combatClass.passiveEffects);
        DetachEffects(combatClass.activeEffects);
        combatClass.OnUnequip(this);
        combatClass = null;
        RecalculateCurrentHealthAfterMaxChange();
    }

    public void EquipWeapon(WeaponResource newWeapon)
    {
        UnequipWeapon();
        weapon = newWeapon;
        if (weapon == null)
            return;
        equipmentBonuses.Add(weapon.weaponBonuses);
        AttachEffects(weapon.passiveEffects);
        AttachEffects(weapon.activeEffects);
        RecalculateCurrentHealthAfterMaxChange();
    }

    public void UnequipWeapon()
    {
        if (weapon == null)
            return;
        equipmentBonuses = Subtract(equipmentBonuses, weapon.weaponBonuses);
        DetachEffects(weapon.passiveEffects);
        DetachEffects(weapon.activeEffects);
        weapon = null;
        RecalculateCurrentHealthAfterMaxChange();
    }

    public void EquipTrinket(TrinketResource newTrinket)
    {
        UnequipTrinket();
        if (
            newTrinket == null
            || (
                combatClass != null
                && newTrinket.allowedClassIds.Count > 0
                && !newTrinket.allowedClassIds.Contains(combatClass.id)
            )
        )
            return;
        trinket = newTrinket;
        equipmentBonuses.Add(trinket.trinketBonuses);
        AttachEffects(trinket.passiveEffects);
        AttachEffects(trinket.activeEffects);
        RecalculateCurrentHealthAfterMaxChange();
    }

    public void UnequipTrinket()
    {
        if (trinket == null)
            return;
        equipmentBonuses = Subtract(equipmentBonuses, trinket.trinketBonuses);
        DetachEffects(trinket.passiveEffects);
        DetachEffects(trinket.activeEffects);
        trinket = null;
        RecalculateCurrentHealthAfterMaxChange();
    }

    public void EquipSkill(SkillResource skill, SkillSlot slot)
    {
        switch (slot)
        {
            case SkillSlot.Offensive:
                offensiveSkill = skill;
                break;
            case SkillSlot.Defensive:
                defensiveSkill = skill;
                break;
            case SkillSlot.Support:
                supportSkill = skill;
                break;
            case SkillSlot.Universal:
                universalSkill = skill;
                break;
        }
    }

    public void UnequipSkill(SkillSlot slot) => EquipSkill(null, slot);

    public bool ActivateSkill(SkillSlot slot, Stats foe)
    {
        SkillResource skill = slot switch
        {
            SkillSlot.Offensive => offensiveSkill,
            SkillSlot.Defensive => defensiveSkill,
            SkillSlot.Support => supportSkill,
            SkillSlot.Universal => universalSkill,
            _ => null,
        };
        if (skill?.effect == null || skill.isPassive || !skill.effect.CanActivate(this, foe))
            return false;
        skill.effect.Execute(this, foe);
        return true;
    }

    public void EvaluatePassiveEffects(Stats foe)
    {
        foreach (EffectResource effect in attachedEffects)
            if (effect.isPassive && effect.CanActivate(this, foe))
                effect.Execute(this, foe);

        foreach (
            SkillResource skill in new[]
            {
                offensiveSkill,
                defensiveSkill,
                supportSkill,
                universalSkill,
            }
        )
            if (
                skill?.isPassive == true
                && skill.effect != null
                && skill.effect.CanActivate(this, foe)
            )
                skill.effect.Execute(this, foe);
    }

    private void AttachEffects(Godot.Collections.Array<Resource> resources)
    {
        foreach (Resource resource in resources)
            if (resource is EffectResource effect && !attachedEffects.Contains(effect))
                attachedEffects.Add(effect);
    }

    private void DetachEffects(Godot.Collections.Array<Resource> resources)
    {
        foreach (Resource resource in resources)
            if (resource is EffectResource effect)
                attachedEffects.Remove(effect);
    }

    private static StatSet Subtract(StatSet left, StatSet right)
    {
        StatSet result = left?.Duplicate() ?? new StatSet();
        if (right == null)
            return result;
        foreach (StatType stat in Enum.GetValues<StatType>())
            result.Set(stat, result.Get(stat) - right.Get(stat));
        return result;
    }
}
