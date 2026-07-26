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
        UnloadLevel();
        string levelPath = $"res://stages/tactics/test/{levelName}.tscn";
        var levelInstance = GD.Load<PackedScene>(levelPath).Instantiate<TacticsLevel>();
        world.AddChild(levelInstance);
        GetNode<CenterContainer>("UI/MapSelector").Visible = false;
    }

    private void UnloadLevel()
    {
        if (IsInstanceValid(levelInstance))
            levelInstance.QueueFree();
        levelInstance = null;
    }
    #endregion
}
