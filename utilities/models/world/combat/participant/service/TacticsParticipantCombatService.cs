using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

public partial class TacticsParticipantCombatService : RefCounted
{
    TacticsParticipantResource resource;
    TacticsCameraResource camera;
    TacticsControlsResource controls;

    public TacticsParticipantCombatService(
        TacticsParticipantResource _resource,
        TacticsCameraResource _camera,
        TacticsControlsResource _controls
    )
    {
        resource = _resource;
        camera = _camera;
        controls = _controls;
    }

    public void AttackPawn(double delta, bool isPlayer)
    {
        if (resource.attackablePawn != null)
        {
            resource.currentPawn.resource.canAttack = false;
        }
        else { }
    }
}
