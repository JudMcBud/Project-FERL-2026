using System;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

[GlobalClass]
public partial class TacticsParticipantResource : Resource
{
    #region Stage Selection
    public enum Stage
    {
        SelectPawn,
        ShowActions,
        ShowMovements,
        SelectLocation,
        MovePawn,
        DisplayTargets,
        SelectAttackTarget,
        Attack,
    }

    public Stage stage = Stage.SelectPawn;
    #endregion

    public TacticsPawn currentPawn = null;
    public Node targets = null;
    public bool turnJustStarted = true;
}
