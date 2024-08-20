using System.Diagnostics;
using System.Linq;

public partial class CheckpointHandler : Node
{
    [Export]
    public Area2D? DetectArea;

    [Export]
    public Checkpoint[] CheckpointsVisited = [];

    [Export]
    public Player? Player;

    public override void _Ready()
    {
        Debug.Assert(DetectArea != null);
        DetectArea.AreaEntered += OnDetectAreaEntered;
    }

    int currentCheckpoint = 0;

    public override void _Process(double delta)
    {
        Debug.Assert(Player != null);

        var hasNextCheckpoint = currentCheckpoint < CheckpointsVisited.Length - 1;
        if (Input.IsActionJustPressed(KeyMap.CheckpointPrev))
        {
            if (currentCheckpoint > 0) currentCheckpoint -= 1;

            Player.GlobalPosition = CheckpointsVisited[currentCheckpoint].GlobalPosition;
        }
        else if (Input.IsActionJustPressed(KeyMap.CheckpointNext) && hasNextCheckpoint)
        {
            currentCheckpoint += 1;

            Player.GlobalPosition = CheckpointsVisited[currentCheckpoint].GlobalPosition;
        }
    }

    void OnDetectAreaEntered(Node2D area)
    {
        if (area is not Checkpoint checkpoint) return;

        if (checkpoint.Visited && CheckpointsVisited.Contains(checkpoint)) return;

        checkpoint.Visit();
        CheckpointsVisited = CheckpointsVisited.Append(checkpoint).ToArray();
        currentCheckpoint = CheckpointsVisited.Length;
    }
}
