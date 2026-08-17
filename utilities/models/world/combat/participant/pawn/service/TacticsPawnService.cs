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

    public void Process(TacticsPawn pawn, double delta)
    {
        // Uncomment for excessive logging
        // GD.Print($"[TacticsPawnService] Process called for {pawn.Name}");
        pawn.GetNode<TacticsPawnSprite>("Character").RotateSprite(pawn.GlobalBasis);
        movement.MoveAlongPath(pawn, delta);
        animation.StartAnimator(pawn);
        ui.TintWhenUnableToAct(pawn);
        ui.UpdateCharacterHealth(pawn);
    }

    public bool AttackTargetPawn(TacticsPawn pawn, TacticsPawn targetPawn, double delta)
    {
        return combat.AttackTargetPawn(pawn, targetPawn, delta);
    }
}
