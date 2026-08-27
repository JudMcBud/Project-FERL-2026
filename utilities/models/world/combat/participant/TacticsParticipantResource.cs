using System;
using Game.Modules.Tactics.Level.Pawn.TacticsPawn;
using Godot;

[GlobalClass]
public partial class TacticsParticipantResource : Resource
{
    [Signal]
    public delegate void SkipTurnEventHandler();

    #region Stage Selection
    public enum Stage
    {
        SelectPawn = 0,
        ShowActions = 1,
        ShowMovements = 2,
        SelectLocation = 3,
        MovePawn = 4,
        DisplayTargets = 5,
        SelectAttackTarget = 6,
        Attack = 7,
    }

    public Stage stage = 0;
    #endregion

    public TacticsPawn currentPawn = null;
    public TacticsPawn attackablePawn = null;
    public Node targets = null;

    public bool displayOpponentStats = false;
    public bool turnJustStarted = true;

    public void SkipTurnHandler()
    {
        EmitSignal(SignalName.SkipTurn);
    }
}
