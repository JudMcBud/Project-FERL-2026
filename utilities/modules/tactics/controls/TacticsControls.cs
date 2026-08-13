using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsControls : Control
{
    #region Props
    [Export]
    public TacticsControlsResource controls = GD.Load<TacticsControlsResource>(
        "res://utilities/models/view/control/tactics/control.tres"
    );

    [Export]
    public TacticsCameraResource tacticsCam = GD.Load<TacticsCameraResource>(
        "res://utilities/models/view/camera/tactics/camera.tres"
    );

    [Export]
    public TacticsParticipantResource participant = GD.Load<TacticsParticipantResource>(
        "res://utilities/models/world/combat/participant/participant.tres"
    );

    [Export]
    public TacticsArenaResource arena = GD.Load<TacticsArenaResource>(
        "res://utilities/models/world/combat/arena/tacticsArenaResource.tres"
    );

    public Texture2D layoutXbox = GD.Load<Texture2D>("");
    public Texture2D layoutPC = GD.Load<Texture2D>("");

    public TacticsPawn currentPawn = null;
    public TacticsControlsService service;
    #endregion
}
