using System;
using System.Linq;
using System.Resources;
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

        if (pawn.resource.moveDirection.Y < -TacticsPawnResource.MinHeightToJump)
        {
            TacticsTile _firstTileInStack = pawn.resource.pathfindingTilestack[0];
        }
    }

    public Vector3 CalculateVelocity(TacticsPawn pawn, double delta)
    {
        return new Vector3();
    }

    public void ResetMovementState(TacticsPawn pawn) { }

    public void CheckMovementCompletion(TacticsPawn pawn) { }
}
