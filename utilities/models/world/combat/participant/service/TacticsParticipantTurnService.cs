using System;
using System.Linq;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Models.World.Combat.Participants.TacticsParticipant;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsParticipantTurnService : RefCounted
{
    private TacticsParticipantResource resource;
    private TacticsCameraResource camera;
    private TacticsControlsResource controls;

    public TacticsParticipantTurnService(
        TacticsParticipantResource _resource,
        TacticsCameraResource _camera,
        TacticsControlsResource _controls
    )
    {
        resource = _resource;
        camera = _camera;
        controls = _controls;
    }

    public void HandlePlayerTurn(double delta, TacticsPlayer player, TacticsParticipant participant)
    {
        if (resource.turnJustStarted)
        {
            Godot.Collections.Array<Node> playerChildren = player.GetChildren();
            Node3D firstPlayerChild = (Node3D)playerChildren.First();
            camera.target = firstPlayerChild;
            resource.turnJustStarted = false;
        }

        controls.MoveCameraHandler(delta);
        bool showActionsMenu =
            resource.stage == TacticsParticipantResource.Stage.ShowActions
            || resource.stage == TacticsParticipantResource.Stage.ShowMovements
            || resource.stage == TacticsParticipantResource.Stage.SelectLocation
            || resource.stage == TacticsParticipantResource.Stage.DisplayTargets
            || resource.stage == TacticsParticipantResource.Stage.SelectAttackTarget;
        controls.SetActionsMenuVisibilityHandler(showActionsMenu, resource.currentPawn);

        switch (resource.stage)
        {
            case TacticsParticipantResource.Stage.SelectPawn:
                controls.SelectPawnHandler(player);
                break;
            case TacticsParticipantResource.Stage.ShowActions:
                player.ShowAvailablePawnActions();
                break;
            case TacticsParticipantResource.Stage.ShowMovements:
                player.ShowAvailableMovements();
                break;
            case TacticsParticipantResource.Stage.SelectLocation:
                controls.SelectNewLocationHandler();
                break;
            case TacticsParticipantResource.Stage.MovePawn:
                player.MovePawn();
                break;
            case TacticsParticipantResource.Stage.DisplayTargets:
                player.DisplayAttackableTargets();
                break;
            case TacticsParticipantResource.Stage.SelectAttackTarget:
                controls.SelectPawnToAttackHandler();
                break;
            case TacticsParticipantResource.Stage.Attack:
                participant.service.combatService.AttackPawn(delta, true);
                break;
        }
    }

    public void HandleOpponentTurn(
        double delta,
        TacticsOpponent opponent,
        TacticsParticipant participant
    )
    {
        resource.targets = participant.GetNode("%TacticsPlayer");
        controls.SetActionsMenuVisibilityHandler(false, null);
        if ((int)resource.stage > 4)
            resource.stage = 0;

        switch (resource.stage)
        {
            case TacticsParticipantResource.Stage.SelectPawn:
                opponent.ChoosePawn();
                break;
            case TacticsParticipantResource.Stage.ShowActions:
                opponent.ChaseNearestEnemy();
                break;
            case TacticsParticipantResource.Stage.ShowMovements:
                opponent.IsPawnDoneMoving();
                break;
            case TacticsParticipantResource.Stage.SelectLocation:
                opponent.ChoosePawnToAttack();
                break;
            case TacticsParticipantResource.Stage.MovePawn:
                participant.service.combatService.AttackPawn(delta, false);
                break;
        }
    }

    public bool CanAct(Node3D parent)
    {
        foreach (TacticsPawn p in parent.GetChildren())
        {
            if (p.CanAct())
                return true;
        }
        return false;
    }

    public void ResetTurn(Node3D parent)
    {
        resource.turnJustStarted = true;
        foreach (TacticsPawn p in parent.GetChildren())
            p.ResetTurn();
    }

    public void SkipTurn(TacticsPlayer player)
    {
        foreach (TacticsPawn pawn in player.GetChildren())
            pawn.EndPawnTurn();
        resource.stage = TacticsParticipantResource.Stage.SelectPawn;
    }
}
