using System;
using Godot;

public partial class CursorService : RefCounted
{
    public static void SetCursorShapeToMove()
    {
        if (Input.GetCurrentCursorShape() != Input.CursorShape.Move)
            Input.SetDefaultCursorShape(Input.CursorShape.Move);
    }

    public static void SetCursorShapeToArrow()
    {
        if (Input.GetCurrentCursorShape() != Input.CursorShape.Arrow)
            Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
    }
}
