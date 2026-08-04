using System;
using System.Linq;
using System.Resources;
using Game.Models.World.Utilities.CalcVector;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsPawnMovementService : RefCounted
{
    public void LookAtDirection(TacticsPawn pawn, Vector3 direction)
    {
        Vector3 _fixedDirection =
            Math.Abs(direction.X) > Math.Abs(direction.Y)
                ? direction * new Vector3(1, 0, 0)
                : new Vector3(0, 0, 1);
        float _angle =
            Vector3.Forward.SignedAngleTo(_fixedDirection.Normalized(), Vector3.Up)
            + (float)Math.PI;
        Vector3 _newRotation = Vector3.Up * _angle;
        pawn.SetRotation(_newRotation);
    }

    public void MoveAlongPath(TacticsPawn pawn, double delta)
    {
        if (pawn.resource.pathfindingTilestack.Count == 0 || !pawn.resource.canMove)
            return;

        StartMovement(pawn);

        if (pawn.resource.moveDirection.Length() > 0.5)
        {
            PerformMovement(pawn, delta);

            // The way the distance is calculated here could cause some problems in the future
            TacticsTile _firstInTilestack = pawn.resource.pathfindingTilestack[0];
            if (pawn.GlobalPosition.DistanceTo(_firstInTilestack.GlobalPosition) >= 0.15)
                return;
        }

        pawn.resource.pathfindingTilestack.RemoveAt(0);
        ResetMovementState(pawn);
        CheckMovementCompletion(pawn);
    }

    public void StartMovement(TacticsPawn pawn)
    {
        pawn.resource.SetMoving(true);
        if (pawn.resource.moveDirection == Vector3.Zero)
        {
            pawn.resource.moveDirection =
                pawn.resource.pathfindingTilestack[0].GlobalPosition - pawn.GlobalPosition;
        }
    }

    public void PerformMovement(TacticsPawn pawn, double delta)
    {
        LookAtDirection(pawn, pawn.resource.moveDirection);
        Vector3 _pVelocity = CalculateVelocity(pawn, delta);
        float _currentSpeed = CalculateSpeed(pawn);

        pawn.Velocity = _pVelocity * _currentSpeed;
        pawn.UpDirection = Vector3.Up;
        pawn.MoveAndSlide();
    }

    public Vector3 CalculateVelocity(TacticsPawn pawn, double delta)
    {
        Vector3 _pVelocity = pawn.resource.moveDirection.Normalized();

        if (pawn.resource.moveDirection.Y < -TacticsPawnResource.MinHeightToJump)
        {
            TacticsTile _firstTileInStack = pawn.resource.pathfindingTilestack[0];
            if (
                CalcVector.DistanceWithoutY(_firstTileInStack.GlobalPosition, pawn.GlobalPosition)
                <= 0.2
            )
            {
                pawn.resource.gravity +=
                    Vector3.Down * (float)delta * TacticsPawnResource.GravityStrength;
                _pVelocity =
                    (
                        pawn.resource.pathfindingTilestack[0].GlobalPosition - pawn.GlobalPosition
                    ).Normalized() + pawn.resource.gravity;
            }
            else
            {
                _pVelocity = CalcVector.RemoveY(pawn.resource.moveDirection).Normalized();
            }
        }
        return _pVelocity;
    }

    public float CalculateSpeed(TacticsPawn pawn)
    {
        float _currentSpeed = pawn.resource.walkSpeed;

        if (pawn.resource.moveDirection.Y > TacticsPawnResource.MinHeightToJump)
        {
            _currentSpeed = Math.Clamp(
                Math.Abs(pawn.resource.moveDirection.Y) * 2.3f,
                3,
                float.PositiveInfinity
            );
            pawn.resource.isJumping = true;
        }
        return _currentSpeed;
    }

    public void ResetMovementState(TacticsPawn pawn)
    {
        pawn.resource.moveDirection = Vector3.Zero;
        pawn.resource.isJumping = false;
        pawn.resource.gravity = Vector3.Zero;
        pawn.resource.canMove = pawn.resource.pathfindingTilestack.Count > 0;
    }

    public void CheckMovementCompletion(TacticsPawn pawn)
    {
        if (!pawn.resource.canMove)
        {
            pawn.resource.SetMoving(false);
            pawn.character.AdjustToCenter(pawn);
        }
    }
}
