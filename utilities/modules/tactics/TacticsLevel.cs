using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Participants.TacticsParticipant;
using Godot;

namespace Game.Stages.TacticsLevel;

public partial class TacticsLevel : Node3D
{
    #region --- Props ---
    [Export]
    TacticsCameraResource camera = GD.Load<TacticsCameraResource>("");

    [Export]
    float cameraBoundaryRadius = 10;

    [Export]
    TacticsControlsResource uiControl = GD.Load<TacticsControlsResource>("");

    TacticsParticipant participant;

    // TacticsPlayer player = null;

    // TacticsOpponent opponent;

    // TacticsArena arena;

    int turnStage = 0;
    #endregion
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
