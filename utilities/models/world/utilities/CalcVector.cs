using Godot;

namespace Game.Models.World.Utilities.CalcVector;

public partial class CalcVector : Node
{
    public static Vector3 RemoveY(Vector3 vector)
    {
        return vector * new Vector3(1, 0, 1);
    }

    public static float DistanceWithoutY(Vector3 b, Vector3 a)
    {
        return RemoveY(b).DistanceTo(RemoveY(a));
    }
}
