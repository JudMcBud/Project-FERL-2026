using Godot;

namespace Game.Models.View.Camera.Tactics.TacticsCameraResource;

[GlobalClass]
public partial class TacticsCameraResource : Resource
{
    #region Panning
    [Export]
    public float boundaryRadius = 10.0f;
    #endregion
}
