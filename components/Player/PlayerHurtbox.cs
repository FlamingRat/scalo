using System.Diagnostics;

public partial class PlayerHurtbox : Area2D
{
    [Export]
    public Player? Player;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    void OnBodyEntered(Node2D body)
    {
        Debug.Assert(Player != null);

        if (body is not Beetle beetle) return;

        if (Player.Scale.X / beetle.Scale.X <= 2) return;

        beetle.Die();
    }
}
