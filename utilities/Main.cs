using Game.Stages.TacticsLevel;
using Godot;

public partial class Main : Node3D
{
    #region: --- Props ---
    private TacticsLevel levelInstance;
    private Node3D world;

    private Button demoMapButton;
    private Button testDialogueButton;
    private TacticsControls tacticsControls;
    #endregion

    #region --- Processing ---
    public override void _Ready()
    {
        tacticsControls = GetNode<TacticsControls>("TacticsControls");
        tacticsControls.Visible = false;
        world = GetNode<Node3D>("World");
        demoMapButton = GetNode<Button>("UI/MapSelector/VBoxContainer/LoadMap0");
        demoMapButton.Pressed += OnDemoMapButtonPressed;
        demoMapButton.GrabFocus();

        testDialogueButton = GetNode<Button>("UI/MapSelector/VBoxContainer/LoadMap1");
        testDialogueButton.Pressed += OnTestDialogueButtonPressed;
    }
    #endregion

    #region --- Signals ---
    private void OnDemoMapButtonPressed()
    {
        LoadLevel("TestLevel");
    }

    private void OnTestDialogueButtonPressed()
    {
        LoadDialogue();
    }
    #endregion

    #region --- Methods ---
    private void LoadLevel(string levelName)
    {
        // GD.Print("Begin Load Level");
        UnloadLevel();
        // GD.Print("Level Unloaded");
        string levelPath = $"res://stages/tactics/test/{levelName}.tscn";
        levelInstance = GD.Load<PackedScene>(levelPath).Instantiate<TacticsLevel>();
        // GD.Print("levelInstance Loaded");
        world.AddChild(levelInstance);
        // GD.Print("levelInstance Added to world");
        GetNode<CenterContainer>("UI/MapSelector").Visible = false;
        // GD.Print("Button hidden");
    }

    private void LoadDialogue()
    {
        // GD.Print("Begin Load Level");
        UnloadLevel();
        // GD.Print("Level Unloaded");
        string levelPath = $"res://utilities/modules/dialogue/Dialogue.tscn";
        Node3D dialogueInstance = GD.Load<PackedScene>(levelPath).Instantiate<Node3D>();
        // GD.Print("levelInstance Loaded");
        world.AddChild(dialogueInstance);
        // GD.Print("levelInstance Added to world");
        GetNode<CenterContainer>("UI/MapSelector").Visible = false;
        // GD.Print("Button hidden");
    }

    private void UnloadLevel()
    {
        if (IsInstanceValid(levelInstance))
            levelInstance.QueueFree();
        levelInstance = null;
    }
    #endregion
}
