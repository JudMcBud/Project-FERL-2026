using System;
using System.Net.Security;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

namespace Game.Models.World.Combat.Participants.TacticsParticipant;

public partial class TacticsParticipant : Node3D
{
    [Export]
    private TacticsParticipantResource resource = GD.Load<TacticsParticipantResource>(
        "res://utilities/models/world/combat/participant/participant.tres"
    );

    [Export]
    private TacticsCameraResource camera = GD.Load<TacticsCameraResource>(
        "res://utilities/models/view/camera/tactics/camera.tres"
    );

    [Export]
    private TacticsControlsResource controls = GD.Load<TacticsControlsResource>(
        "res://utilities/models/view/control/tactics/control.tres"
    );

    public TacticsParticipantService service;
    private TacticsArena arena;
    private TacticsPlayer player;
    private TacticsOpponent opponent;

    public override void _Ready()
    {
        service = new TacticsParticipantService(resource, camera, controls);
        service.Setup(this);
        resource.SkipTurn += OnSkipTurn;
        arena = GetNode<TacticsArena>("%TacticsArena");
        player = GetNode<TacticsPlayer>("%TacticsPlayer");
        opponent = GetNode<TacticsOpponent>("%TacticsOpponent");
    }

    public void Act(double delta, bool isPlayer, Node3D parent)
    {
        service.Act(delta, isPlayer, parent, this);
    }

    public void Configure(TacticsCameraResource myCamera, TacticsControlsResource myControls)
    {
        service.Configure(myCamera, myControls);
    }

    public bool IsConfigured(Node3D parent)
    {
        return service.IsConfigured(parent);
    }

    public bool CanAct(Node3D parent)
    {
        return service.CanAct(parent);
    }

    public void ResetTurn(Node3D parent)
    {
        service.ResetTurn(parent);
    }

    private void OnSkipTurn()
    {
        service.SkipTurn(player);
    }
}
