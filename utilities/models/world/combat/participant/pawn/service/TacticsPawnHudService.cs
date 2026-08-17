using System;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsPawnHudService : RefCounted
{
    public void UpdateCharacterHealth(TacticsPawn pawn)
    {
        // Uncomment for excessive logging
        // GD.Print($"[TacticsPawnHudService] UpdateCharacterHealth for {pawn.Name}: {pawn.stats.currentHealth}/{pawn.stats.maxHealth}");
        Label3D _healthLabel = pawn.GetNode<Label3D>("Character/CharacterUI/HealthLabel");
        _healthLabel.Text =
            pawn.stats.currentHealth.ToString() + "/" + pawn.stats.maxHealth.ToString();
    }

    public void TintWhenUnableToAct(TacticsPawn pawn)
    {
        // Uncomment for excessive logging
        // GD.Print($"[TacticsPawnHudService] TintWhenUnableToAct for {pawn.Name}, canAct: {pawn.CanAct()}");
        TacticsPawnSprite _characterNode = pawn.GetNode<TacticsPawnSprite>("Character");
        _characterNode.Modulate = !pawn.CanAct()
            ? new Color(0.5f, 0.5f, 0.5f)
            : new Color(1f, 1f, 1f);
    }
}
