using System;
using Game.Models.World.Combat.Participants.TacticsParticipant;
using Godot;

public partial class TacticsPlayer : TacticsParticipant
{
    TacticsPlayerService playerService;

    public override void _Ready()
    {
        GD.Print("[TacticsPlayer] _Ready begin");
        base._Ready();
        GD.Print("[TacticsPlayer] Base ready complete");
        playerService = new TacticsPlayerService(resource, camera, controls, arena);
        GD.Print("[TacticsPlayer] Service created");
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
