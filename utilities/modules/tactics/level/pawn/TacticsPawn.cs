using System;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Models.World.Combat.Participants.Pawn.Service.TacticsPawnService;
using Godot;

namespace Game.Modules.Tactics.Level.Pawn.TacticsPawn;

public partial class TacticsPawn : CharacterBody3D
{
    [Export]
    public TacticsControlsResource controls = GD.Load<TacticsControlsResource>(
        "res://utilities/models/view/control/tactics/control.tres"
    );
    public TacticsPawnResource resource;
    public TacticsPawnService service;
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    public Stats stats;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public TacticsTile GetTile()
    {
        return new TacticsTile();
    }

    public bool CanAct()
    {
        return true;
    }

    public void ResetTurn() { }

    public void EndTurn() { }

    public bool AttackTargetPawn(TacticsPawn targetPawn, double delta)
    {
        return true;
    }
}
