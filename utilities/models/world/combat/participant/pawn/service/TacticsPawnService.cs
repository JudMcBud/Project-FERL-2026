using System;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

namespace Game.Models.World.Combat.Participants.Pawn.Service.TacticsPawnService;

public partial class TacticsPawnService : RefCounted
{
    public TacticsPawnMovementService movement;
    public TacticsPawnCombatService combat;
    public TacticsPawnAnimationService animation;
    public TacticsPawnHudService ui;
    public TacticsPawnSprite character;

    public TacticsPawnService()
    {
        movement = new TacticsPawnMovementService();
        combat = new TacticsPawnCombatService();
        animation = new TacticsPawnAnimationService();
        ui = new TacticsPawnHudService();
    }

    public void Setup(TacticsPawn pawn)
    {
        animation.Setup(pawn);
    }
}
