using System;
using System.Net.Security;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

namespace Game.Modules.Tactics.Level.Participants.TacticsParticipant;

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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // service = TacticsParticipantResource.new(resource, camera, controls);
        // service.Setup(self);
        // resource.SkipTurn += OnSkipTurn;
        arena = GetNode<TacticsArena>("%TacticsArena");
        player = GetNode<TacticsPlayer>("%TacticsPlayer");
        opponent = GetNode<TacticsOpponent>("%TacticsOpponent");
    }

    public void Act(double delta, bool isPlayer, Node3D parent)
    {
        // service.Act(delta, isPlayer, parent, self);
    }

    public void Configure(Resource myCamera, Resource myControls)
    {
        // service.Configure(myCamera, myControls);
    }

    public bool IsConfigured(Node3D parent)
    {
        //return service.IsConfigured(parent);
        return true;
    }

    public bool CanAct(Node3D parent)
    {
        //return service.CanAct(parent);
        return true;
    }

    public void ResetTurn(Node3D parent)
    {
        //service.ResetTurn(parent);
    }

    private void OnSkipTurn()
    {
        // service.SkipTurn(player);
    }
}
