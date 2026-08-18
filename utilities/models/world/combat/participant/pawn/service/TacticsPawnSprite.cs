using System;
using System.Reflection.Metadata.Ecma335;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

public partial class TacticsPawnSprite : Sprite3D
{
    public AnimationNodeStateMachinePlayback animator = null;
    public int currentFrame = 0;

    public AnimationTree animationTree;
    public Label3D characterUINameLabel;

    public override void _Ready()
    {
        animationTree = GetNode<AnimationTree>("AnimationTree");
        characterUINameLabel = GetNode<Label3D>("CharacterUI/NameLabel");
    }

    public void Setup(Stats stats, string expertise)
    {
        // No idea if this works or not so I may need to come back here later if things break
        AnimationNodeStateMachinePlayback playback = (AnimationNodeStateMachinePlayback)
            animationTree.Get("parameters/playback");
        if (playback is AnimationNodeStateMachinePlayback)
            animator = playback;
        else
            return;

        animator.Start("Idle");
        animationTree.Active = true;
        Texture = GD.Load<Texture2D>(stats.sprite);
        characterUINameLabel.Text = stats.overrideName != null ? stats.overrideName : expertise;
    }

    public void StartAnimator(Vector3 moveDirection, bool isJumping)
    {
        if (moveDirection == Vector3.Zero)
        {
            animator.Travel("Idle");
        }
        else if (isJumping)
        {
            animator.Travel("Jump");
        }
    }

    public void RotateSprite(Basis globalBasis)
    {
        Vector3 _cameraForward = -GetViewport().GetCamera3D().GlobalBasis.Z;
        float _scalar = globalBasis.Z.Dot(_cameraForward);
        FlipH = globalBasis.X.Dot(_cameraForward) > 0;

        if (_scalar < 0.306)
        {
            Frame = currentFrame;
        }
        else if (_scalar > 0.306)
        {
            Frame = currentFrame + 1 * TacticsPawnResource.AnimationFrames;
        }
    }

    public bool AdjustToCenter(TacticsPawn pawn)
    {
        if (pawn.GetTile() != null && !pawn.resource.isMoving)
        {
            pawn.GlobalPosition = pawn.GetTile().GlobalPosition;
            return true;
        }
        return false;
    }
}
