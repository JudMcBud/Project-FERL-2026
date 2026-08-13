using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

public partial class TacticsControlsService : RefCounted
{
    public TacticsControlsResource controls;
    public TacticsCameraResource tacticsCam;
    public TacticsParticipantResource participant;
    public TacticsArenaResource arena;
    public Node inputCapture;
    public TacticsControlsInputService inputService;
    public TacticsUIService uiService;
    public TacticsControlsCameraService cameraService;
    public TacticsControlsSelectionService pawnSelectionService;
}
