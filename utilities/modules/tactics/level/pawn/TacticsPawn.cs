using System;
using System.Globalization;
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

    public Stats stats;
    public string expertise;
    public TacticsPawnSprite character;

    public override void _Ready()
    {
        stats = GetNode<Stats>("Expertise/Stats");
        expertise = stats.expertise;
        character = GetNode<TacticsPawnSprite>("Character");

        resource = new TacticsPawnResource();
        service = new TacticsPawnService();
        service.Setup(this);
        controls.SetActionsMenuVisibilityHandler(false, this);
        ShowPawnStats(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        service.Process(this, delta);
    }

    public bool Center()
    {
        return character.AdjustToCenter(this);
    }

    public void ShowPawnStats(bool v)
    {
        GetNode<Node3D>("Character/CharacterUI").Visible = v;
    }

    public TacticsTile GetTile()
    {
        return (TacticsTile)GetNode<RayCast3D>("Tile").GetCollider();
    }

    public bool IsAlive()
    {
        return stats.currentHealth > 0;
    }

    public bool CanPawnAttack()
    {
        return resource.canAttack && IsAlive();
    }

    public bool CanAct()
    {
        return (resource.canMove || resource.canAttack) && IsAlive();
    }

    public void ResetTurn()
    {
        resource.ResetTurn();
    }

    public void EndPawnTurn()
    {
        resource.EndPawnTurn();
    }

    public bool AttackTargetPawn(TacticsPawn targetPawn, double delta)
    {
        return service.AttackTargetPawn(this, targetPawn, delta);
    }

    public void MoveAlongPath(double delta)
    {
        service.movement.MoveAlongPath(this, delta);
    }
}
