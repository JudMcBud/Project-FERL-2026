using System;
using Game.Models.View.Control.Tactics.TacticsControlsResource;
using Godot;

public partial class InputHints : Container
{
    public const float AnimationDuration = 0.3f;
    public const float FoldedOffset = -514.0f;

    public TacticsControlsResource resource;

    private static TacticsControlsResource LoadControlsResource(string path)
    {
        return ResourceLoader.Load<TacticsControlsResource>(path) ?? new TacticsControlsResource();
    }

    public Control controllerHints;

    public override void _Ready()
    {
        resource = LoadControlsResource("res://utilities/models/view/control/tactics/control.tres");
        controllerHints = GetNode<Control>("%ControllerHints");

        resource.inputHintsFolded = true;
        UpdateHintsVisibility(true);

        controllerHints.MouseEntered += OnMouseEntered;
        controllerHints.MouseExited += OnMouseExited;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("controllerHints"))
            OnMouseEntered();
        else if (@event.IsActionReleased("controllerHints"))
            OnMouseExited();
    }

    private void OnMouseEntered()
    {
        resource.inputHintsFolded = false;
        UpdateHintsVisibility();
    }

    private void OnMouseExited()
    {
        resource.inputHintsFolded = true;
        UpdateHintsVisibility();
    }

    public void UpdateHintsVisibility(bool forceImmediate = false)
    {
        float targetX = resource.inputHintsFolded ? FoldedOffset : 0.0f;
        float targetAlpha = resource.inputHintsFolded ? 0.3f : 1.0f;

        if (forceImmediate)
        {
            controllerHints.Position = new Vector2(targetX, controllerHints.Position.Y);
            controllerHints.Modulate = new Color(controllerHints.Modulate, targetAlpha);
        }
        else
        {
            Tween tween = CreateTween();
            tween.SetParallel(true);
            tween.SetTrans(Tween.TransitionType.Sine);
            tween.SetEase(Tween.EaseType.InOut);
            tween.TweenProperty(controllerHints, "Position:X", targetX, AnimationDuration);
            tween.TweenProperty(controllerHints, "Modulate:A", targetAlpha, AnimationDuration);
        }
    }
}
