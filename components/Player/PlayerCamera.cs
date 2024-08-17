[Tool]
public partial class PlayerCamera : Camera2D
{
    [Export]
    public Node2D Player;

    public override void _Process(double delta)
    {
        Zoom = Vector2.One / Player.Scale * 2f;
    }
}
