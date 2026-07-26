using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Participants.TacticsParticipant;
using Godot;

public partial class TacticsParticipantService : RefCounted
{
    private TacticsParticipantResource resource;
    private TacticsCameraResource camera;
    private TacticsControlsResource controls;
    private TacticsParticipantTurnService turnService;
    public TacticsParticipantCombatService combatService;

    public void Init(
        TacticsParticipantResource _resource,
        TacticsCameraResource _camera,
        TacticsControlsResource _controls
    )
    {
        resource = _resource;
        camera = _camera;
        controls = _controls;
        turnService = new TacticsParticipantTurnService(resource, camera, controls);
        combatService = new TacticsParticipantCombatService(resource, camera, controls);
    }

    public void Setup(TacticsParticipant _participant)
    {
        if (controls == null)
            GD.PushError(
                "TacticsControls needs a ControlResource from /utilities/models/view/control/tactics/"
            );
        if (camera == null)
            GD.PushError(
                "TacticsControls needs a CameraResource from /utilities/models/view/camera/tactics/"
            );
        if (resource == null)
            GD.PushError(
                "TacticsControls needs a ParticipantResource from /utilities/models/world/combat/participant/"
            );
    }

    public void Act(float delta, bool isPlayer, Node3D parent, TacticsParticipant participant)
    {
        if (isPlayer)
        {
            TacticsPlayer player = parent as TacticsPlayer;
            turnService.HandlePlayerTurn(delta, player, participant);
        }
        else
        {
            TacticsOpponent opponent = parent as TacticsOpponent;
            turnService.HandleOpponentTurn(delta, opponent, participant);
        }
    }

    public void Configure(TacticsCameraResource myCamera, TacticsControlsResource myControls)
    {
        camera = myCamera;
        controls = myControls;
    }

    public bool IsConfigured(Node3D parent)
    {
        TacticsParticipant participant = parent as TacticsParticipant;
        return participant.IsConfigured(parent);
    }

    public bool CanAct(Node3D parent)
    {
        TacticsParticipant participant = parent as TacticsParticipant;
        return participant.CanAct(parent);
    }

    public void ResetTurn(Node3D parent)
    {
        turnService.ResetTurn(parent);
    }

    public void SkipTurn(TacticsPlayer player)
    {
        turnService.SkipTurn(player);
    }
}
