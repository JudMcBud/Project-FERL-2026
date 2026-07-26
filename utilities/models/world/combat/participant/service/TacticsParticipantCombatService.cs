using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

public partial class TacticsParticipantCombatService : RefCounted
{
    public TacticsParticipantCombatService(
        TacticsParticipantResource resource,
        TacticsCameraResource camera,
        TacticsControlsResource controls
    ) { }

    public void AttackPawn(double delta, bool isPlayer) { }
}
