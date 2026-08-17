using System;
using System.Net.Security;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

namespace Game.Models.World.Combat.Participants.TacticsParticipant;

public partial class TacticsParticipant : Node3D
{
    [Export]
    public TacticsParticipantResource resource;

    [Export]
    public TacticsCameraResource camera;

    [Export]
    public TacticsControlsResource controls;

    private static TacticsParticipantResource LoadParticipantResource(string path)
    {
        return ResourceLoader.Load<TacticsParticipantResource>(path)
            ?? new TacticsParticipantResource();
    }

    private static TacticsCameraResource LoadCameraResource(string path)
    {
        return ResourceLoader.Load<TacticsCameraResource>(path) ?? new TacticsCameraResource();
    }

    private static TacticsControlsResource LoadControlsResource(string path)
    {
        return ResourceLoader.Load<TacticsControlsResource>(path) ?? new TacticsControlsResource();
    }

    public TacticsParticipantService service;
    public TacticsArena arena;
    public TacticsPlayer player;
    public TacticsOpponent opponent;

    public override void _Ready()
    {
        GD.Print("[TacticsParticipant] _Ready begin");
        resource = LoadParticipantResource(
            "res://utilities/models/world/combat/participant/participant.tres"
        );
        camera = LoadCameraResource("res://utilities/models/view/camera/tactics/camera.tres");
        controls = LoadControlsResource("res://utilities/models/view/control/tactics/control.tres");
        GD.Print("[TacticsParticipant] Resources loaded");

        service = new TacticsParticipantService(resource, camera, controls);
        GD.Print("[TacticsParticipant] Service created");
        service.Setup(this);
        resource.SkipTurn += OnSkipTurn;
        GD.Print("[TacticsParticipant] Signals hooked");
        arena = GetNode<TacticsArena>("%TacticsArena");
        player = GetNode<TacticsPlayer>("%TacticsPlayer");
        opponent = GetNode<TacticsOpponent>("%TacticsOpponent");
        GD.Print("[TacticsParticipant] _Ready complete");
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
        _ = parent;
        return service != null && service.IsConfigured(this);
    }

    public bool CanAct(Node3D parent)
    {
        return service != null && service.CanAct(parent);
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
