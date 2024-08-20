using System.Diagnostics;

public partial class Beetle : CharacterBody2D
{
    [Export]
    public PackedScene? DeathMessage;

    public const float Speed = 300.0f;

    int direction = -1;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (!IsOnFloor()) velocity += GetGravity() * (float)delta;
        if (IsOnWall())
        {
            direction = -direction;
            Scale *= new Vector2(-1, 1);
        }

        velocity.X = direction * Speed;

        Velocity = velocity;
        MoveAndSlide();
    }

    public void Die()
    {
        Debug.Assert(DeathMessage != null);

        var label = DeathMessage.Instantiate<Control>();
        GetParent().AddChild(label);
        label.GlobalPosition = GlobalPosition;

        QueueFree();
    }
}
