using System;
using System.Net.Security;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

namespace Game.Modules.Tactics.Level.Participants.TacticsParticipant;

public partial class TacticsParticipant : Node3D
{
    // [Export]
    // TacticsParticipantResource res;

    [Export]
    private TacticsCameraResource camera;

    [Export]
    private TacticsControlsResource controls;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    public void Configure(TacticsCameraResource camera, TacticsControlsResource controls)
    {
        //TODO: Finish method
    }

    public bool IsConfigured(Node3D parent)
    {
        //TODO: Finish method
        return true;
    }

    public bool CanAct(Node3D parent)
    {
        //TODO: Finish method
        return true;
    }

    public void Act(double delta, bool isPlayer, Node3D parent)
    {
        //TODO: Finish method
    }

    public void ResetTurn(Node3D parent)
    {
        //TODO: Finish method
    }
}
