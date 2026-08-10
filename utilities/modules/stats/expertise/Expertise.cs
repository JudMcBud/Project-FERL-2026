using System;
using Godot;
using Godot.Collections;

public partial class Expertise : Node
{
    [Export]
    public StatsResource startingStats;

    [Export]
    public Array<string> startingSkills;

    public Stats stats;

    public override void _Ready()
    {
        stats = GetNode<Stats>("Stats");

        if (startingStats == null)
        {
            GD.PushError(
                "Expertise needs a StatsResource (Starting Stats) from /data/models/stats"
            );
        }
        stats.ImportStats(startingStats);
    }
}
