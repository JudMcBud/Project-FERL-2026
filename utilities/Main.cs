using Game.Stages.TacticsLevel;
using Godot;

public partial class Main : Node3D
{
    #region: --- Props ---
    private TacticsLevel levelInstance;
    private Node3D world;

    private Button demoMapButton;
    #endregion

    #region --- Processing ---
    public override void _Ready()
    {
        world = GetNode<Node3D>("World");
        demoMapButton = GetNode<Button>("UI/MapSelector/LoadMap0");
        demoMapButton.Pressed += OnDemoMapButtonPressed;
        demoMapButton.GrabFocus();
    }
    #endregion

    #region --- Signals ---
    private void OnDemoMapButtonPressed()
    {
        LoadLevel("TestLevel");
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

    private void UnloadLevel()
    {
        if (IsInstanceValid(levelInstance))
            levelInstance.QueueFree();
        levelInstance = null;
    }
    #endregion
}
