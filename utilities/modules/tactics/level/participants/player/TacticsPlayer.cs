using System;
using Game.Models.World.Combat.Participants.TacticsParticipant;
using Godot;

public partial class TacticsPlayer : TacticsParticipant
{
    TacticsPlayerService playerService;

    public override void _Ready()
    {
        base._Ready();
        playerService = new TacticsPlayerService(resource, camera, controls, arena);
    }

    public override void _PhysicsProcess(double delta)
    {
        playerService.ToggleEnemyStats(GetNode("%TacticsOpponent"));
    }

    public bool IsPawnConfigured()
    {
        return playerService.IsPawnConfigured(this);
    }

    public void ShowAvailablePawnActions()
    {
        playerService.ShowAvailablePawnActions();
    }

    public void ShowAvailableMovements()
    {
        playerService.ShowAvailableMovements();
    }

    public void DisplayAttackableTargets()
    {
        playerService.DisplayAttackableTargets();
    }

    public void MovePawn()
    {
        playerService.MovePawn();
    }
}
