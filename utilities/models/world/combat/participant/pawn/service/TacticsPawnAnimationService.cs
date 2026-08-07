using System;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsPawnAnimationService : RefCounted
{
    public void Setup(TacticsPawn pawn)
    {
        pawn.GetNode<TacticsPawnSprite>("Character").Setup(pawn.stats, pawn.expertise);
    }

    public void StartAnimator(TacticsPawn pawn)
    {
        pawn.GetNode<TacticsPawnSprite>("Character")
            .StartAnimator(pawn.resource.moveDirection, pawn.resource.isJumping);
    }
}
