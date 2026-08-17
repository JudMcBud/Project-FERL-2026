using System;
using Game.Models.View.Camera.Tactics.TacticsCameraResource;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsControls : Control
{
    #region Props
    [Export]
    public TacticsControlsResource controls;

    [Export]
    public TacticsCameraResource tacticsCam;

    [Export]
    public TacticsParticipantResource participant;

    [Export]
    public TacticsArenaResource arena;

    public TacticsPawn currentPawn = null;

    private static TacticsControlsResource LoadControlsResource(string path)
    {
        return ResourceLoader.Load<TacticsControlsResource>(path) ?? new TacticsControlsResource();
    }

    private static TacticsCameraResource LoadCameraResource(string path)
    {
        return ResourceLoader.Load<TacticsCameraResource>(path) ?? new TacticsCameraResource();
    }

    private static TacticsParticipantResource LoadParticipantResource(string path)
    {
        return ResourceLoader.Load<TacticsParticipantResource>(path)
            ?? new TacticsParticipantResource();
    }

    private static TacticsArenaResource LoadArenaResource(string path)
    {
        return ResourceLoader.Load<TacticsArenaResource>(path) ?? new TacticsArenaResource();
    }

    public TacticsControlsService service;

    public Texture2D layoutXbox = GD.Load<Texture2D>(
        "res://assets/textures/ui/labels/controlsHints/controls-ui-xbox.png"
    );
    public Texture2D layoutPC = GD.Load<Texture2D>(
        "res://assets/textures/ui/labels/controlsHints/controls-ui.png"
    );

    InputCapture inputCapture;
    #endregion

    #region --- Processing ---
    public override void _Ready()
    {
        GD.Print("[TacticsControls] _Ready begin");
        controls = LoadControlsResource("res://utilities/models/view/control/tactics/control.tres");
        tacticsCam = LoadCameraResource("res://utilities/models/view/camera/tactics/camera.tres");
        participant = LoadParticipantResource(
            "res://utilities/models/world/combat/participant/participant.tres"
        );
        arena = LoadArenaResource(
            "res://utilities/models/world/combat/arena/tacticsArenaResource.tres"
        );
        GD.Print("[TacticsControls] Resources loaded");

        inputCapture = GetNode<InputCapture>("InputCapture");
        GD.Print("[TacticsControls] InputCapture found");

        service = new TacticsControlsService(
            controls,
            tacticsCam,
            participant,
            arena,
            inputCapture
        );
        GD.Print("[TacticsControls] Service created");
        service.Setup(this);
        GD.Print("[TacticsControls] Service setup complete");

        foreach (string action in controls.actions.Keys)
        {
            StringName stringName = controls.actions[action];
            GetAct(action).Pressed += () => Call(stringName);
        }
        GD.Print("[TacticsControls] _Ready complete");
    }

    public override void _PhysicsProcess(double delta)
    {
        service.PhysicsProcess(delta, this);
    }

    public override void _Input(InputEvent @event)
    {
        service.HandleInput(@event);
    }
    #endregion

    #region --- Methods ---
    public void SetCursorShapeToMove()
    {
        CursorService.SetCursorShapeToMove();
    }

    public void SetCursorShapeToArrow()
    {
        CursorService.SetCursorShapeToArrow();
    }

    public void MoveCamera(double delta)
    {
        // This code is dead in the original project
        service.cameraService.MoveCamera(delta, controls.isJoystick);
    }

    public Button GetAct(string action = "")
    {
        // Might be a bug here because %Actions doesn't return a button but a BoxContainer
        if (action == "")
            return GetNode<Button>("%Actions");
        return GetNode<BoxContainer>("%Actions").GetNode<Button>(action);
    }

    public bool IsMouseHoveringUiElem()
    {
        // This code is also dead in the original project
        return service.inputService.IsMouseHoveringUIElem(this);
    }

    public void SetActionsMenuVisibility(bool v, TacticsPawn p)
    {
        service.SetActionsMenuVisibility(v, p, this);
    }

    public Object Get3DCanvasMousePosition(int collisionMask)
    {
        return service.inputService.Get3DCanvasMousePosition(collisionMask, this);
    }

    public void SelectPawn(TacticsPlayer player)
    {
        service.SelectPawn(player, this);
    }

    public void SelectNewLocation()
    {
        service.SelectNewLocation(this);
    }

    public void SelectPawnToAttack()
    {
        service.SelectPawnToAttack(this);
    }

    public void PlayerWantsToMove()
    {
        service.PlayerWantsToMove();
    }

    public void PlayerWantsToCancel()
    {
        service.PlayerWantsToCancel();
    }

    public void PlayerWantsToWait()
    {
        service.PlayerWantsToWait();
    }

    public void PLayerWantsToSkipTurn()
    {
        service.PlayerWantsToSkipTurn();
    }

    public void PlayerWantsToAttack()
    {
        service.PlayerWantsToAttack();
    }
    #endregion
}
