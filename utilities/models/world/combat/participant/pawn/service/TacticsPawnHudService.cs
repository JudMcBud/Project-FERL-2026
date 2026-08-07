using System;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsPawnHudService : RefCounted
{
    public void UpdateCharacterHealth(TacticsPawn pawn)
    {
        Label3D _healthLabel = pawn.GetNode<Label3D>("Character/CharacterUI/HealthLabel");
        _healthLabel.Text =
            pawn.stats.currentHealth.ToString() + "/" + pawn.stats.maxHealth.ToString();
    }

    public void TintWhenUnableToAct(TacticsPawn pawn)
    {
        TacticsPawnSprite _characterNode = pawn.GetNode<TacticsPawnSprite>("Character");
        _characterNode.Modulate = !pawn.CanAct()
            ? new Color(0.5f, 0.5f, 0.5f)
            : new Color(1f, 1f, 1f);
    }
}
