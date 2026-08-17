using System;
using Game.Models.World.Combat.Participants.TacticsParticipant;
using Godot;

public partial class TacticsOpponent : TacticsParticipant
{
    public TacticsOpponentService opponentService;

    public override void _Ready()
    {
        GD.Print("[TacticsOpponent] _Ready begin");
        base._Ready();
        GD.Print("[TacticsOpponent] Base ready complete");
        opponentService = new TacticsOpponentService(resource, camera, controls, arena);
        GD.Print("[TacticsOpponent] Service created");
    }

    public bool IsPawnConfigured()
    {
        return opponentService.IsPawnConfigured(this);
    }

    public void ChoosePawn()
    {
        opponentService.ChoosePawn(this);
    }

    public void ChaseNearestEnemy()
    {
        opponentService.ChaseNearestEnemy(this, GetNode("%TacticsPlayer"));
    }

    public void IsPawnDoneMoving()
    {
        opponentService.IsPawnDoneMoving();
    }

    public void ChoosePawnToAttack()
    {
        opponentService.ChoosePawnToAttack();
    }
}
