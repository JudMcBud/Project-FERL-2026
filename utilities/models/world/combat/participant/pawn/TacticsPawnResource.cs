using System;
using System.Collections.Generic;
using Game.Models.Config.TacticsConfig;
using Godot;

[GlobalClass]
public partial class TacticsPawnResource : Resource
{
    [Signal]
    public delegate void PawnMovedEventHandler();

    [Signal]
    public delegate void PawnAttackedEventHandler();

    [Signal]
    public delegate void TurnEndedEventHandler();

    public const int MinHeightToJump = 1;
    public const int GravityStrength = 6;
    public const float MinTimeForAttack = 1.0f;
    public const int AnimationFrames = 1;

    public bool pawnHudEnabled = false;
    public bool canMove = true;
    public bool canAttack = true;
    public bool isJumping = false;
    public bool isMoving = false;

    public Vector3 moveDirection = Vector3.Zero;
    public List<TacticsTile> pathfindingTilestack = new List<TacticsTile>();
    public Vector3 gravity = Vector3.Zero;
    public float waitDelay = 0.0f;
    public int walkSpeed = TacticsConfig.pawn["baseWalkSpeed"];

    public void ResetTurn()
    {
        canMove = true;
        canAttack = true;
    }

    public void EndPawnTurn()
    {
        canMove = false;
        canAttack = false;
        EmitSignal(SignalName.TurnEnded);
    }

    public void SetMoving(bool value)
    {
        isMoving = value;
        if (value)
        {
            EmitSignal(SignalName.PawnMoved);
        }
    }

    public void SetAttacking(bool value)
    {
        canAttack = value;
        if (value)
        {
            EmitSignal(SignalName.PawnAttacked);
        }
    }
}
