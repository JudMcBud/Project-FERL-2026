using System.Diagnostics;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Models.World.Combat.Participants.TacticsParticipant;
using Godot;

namespace Game.Stages.TacticsLevel;

public partial class TacticsLevel : Node3D
{
    #region --- Props ---
    [Export]
    private TacticsCameraResource camera;

    [Export]
    private float cameraBoundaryRadius = 10;

    [Export]
    private TacticsControlsResource uiControl;

    private static TacticsCameraResource LoadCameraResource(string path)
    {
        return ResourceLoader.Load<TacticsCameraResource>(path) ?? new TacticsCameraResource();
    }

    private static TacticsControlsResource LoadControlsResource(string path)
    {
        return ResourceLoader.Load<TacticsControlsResource>(path) ?? new TacticsControlsResource();
    }

    private TacticsParticipant participant;

    private TacticsPlayer player = null;

    private TacticsOpponent opponent;

    private TacticsArena arena;

    int turnStage = 0;
    #endregion
    #region --- Processing ---
    public override void _Ready()
    {
        GD.Print("[TacticsLevel] _Ready begin");

        uiControl = LoadControlsResource(
            "res://utilities/models/view/control/tactics/control.tres"
        );
        if (uiControl == null)
            GD.PushError("TacticsControls needs a ControlResource from /utilities/models");

        camera = LoadCameraResource("res://utilities/models/view/camera/tactics/camera.tres");
        if (camera == null)
            GD.PushError("TacticaCamera needs a CameraResource from /utilities/models");

        GD.Print("[TacticsLevel] Loaded resources");

        participant = GetNode<TacticsParticipant>("TacticsParticipant");
        player = GetNode<TacticsPlayer>("TacticsParticipant/TacticsPlayer");
        opponent = GetNode<TacticsOpponent>("TacticsParticipant/TacticsOpponent");
        arena = GetNode<TacticsArena>("TacticsArena");
        GD.Print("[TacticsLevel] Got required nodes");

        GD.Print("[TacticsLevel] About to configure tiles");
        arena.ConfigureTiles();
        GD.Print("[TacticsLevel] Tiles configured");

        participant.Configure(camera, uiControl);
        GD.Print("[TacticsLevel] Participant configured");

        if (camera.boundaryRadius != cameraBoundaryRadius)
            camera.boundaryRadius = cameraBoundaryRadius;

        GD.Print("[TacticsLevel] _Ready complete");
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (turnStage)
        {
            case 0:
                initTurn();
                break;

            case 1:
                handleTurn(delta);
                break;
        }
    }

    #endregion

    #region --- Methods ---
    private void initTurn()
    {
        if (participant.IsConfigured(player) && participant.IsConfigured(opponent))
            turnStage = 1;
    }

    private void handleTurn(double delta)
    {
        //Debug Log goes here

        if (participant.CanAct(player))
        {
            if (!participant.IsConfigured(player))
                participant.Configure(camera, uiControl);
            participant.Act(delta, true, player);
        }
        else if (participant.CanAct(opponent))
        {
            if (!participant.IsConfigured(opponent))
                participant.Configure(camera, uiControl);
            participant.Act(delta, false, opponent);
        }
        else
        {
            player.ResetTurn(player);
            opponent.ResetTurn(opponent);
        }
    }
    #endregion
}
