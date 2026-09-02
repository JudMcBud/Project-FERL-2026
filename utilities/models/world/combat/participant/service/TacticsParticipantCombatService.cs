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
        if (resource.currentPawn == null)
        {
            GD.PushWarning("Attempted to attack without a selected pawn.");
            resource.displayOpponentStats = false;
            resource.stage = TacticsParticipantResource.Stage.SelectPawn;
            return;
        }

        if (resource.attackablePawn == null)
        {
            resource.currentPawn.resource.canAttack = false;
            resource.displayOpponentStats = false;
            resource.stage = TacticsParticipantResource.Stage.SelectPawn;
            return;
        }

        if (!resource.currentPawn.AttackTargetPawn(resource.attackablePawn, delta))
            return;

        controls.SetActionsMenuVisibilityHandler(false, resource.attackablePawn);
        camera.target = resource.currentPawn;

        resource.attackablePawn.HandlePawnDeath();
        resource.attackablePawn = null;
        resource.displayOpponentStats = false;

        if (!resource.currentPawn.CanAct() || !isPlayer)
        {
            resource.stage = TacticsParticipantResource.Stage.SelectPawn;
        }
        else if (resource.currentPawn.CanAct() && isPlayer)
        {
            resource.stage = TacticsParticipantResource.Stage.ShowActions;
        }
    }
}
