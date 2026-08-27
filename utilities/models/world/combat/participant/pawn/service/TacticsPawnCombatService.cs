using System;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsPawnCombatService : RefCounted
{
    public bool AttackTargetPawn(TacticsPawn pawn, TacticsPawn targetPawn, double delta)
    {
        if (pawn == null || targetPawn == null || pawn.service?.movement == null)
            return false;

        pawn.service.movement.LookAtDirection(
            pawn,
            targetPawn.GlobalPosition - pawn.GlobalPosition
        );

        if (
            pawn.resource.canAttack
            && pawn.resource.waitDelay > TacticsPawnResource.MinTimeForAttack / 4.0f
        )
        {
            targetPawn.stats.ApplyToCurrentHealth(-pawn.stats.attackPower);
            pawn.resource.SetAttacking(false);
        }

        if (pawn.resource.waitDelay < TacticsPawnResource.MinTimeForAttack)
        {
            pawn.resource.waitDelay += (float)delta;
            return false;
        }

        pawn.resource.waitDelay = 0.0f;
        return true;
    }
}
