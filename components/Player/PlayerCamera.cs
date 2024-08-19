using System.Diagnostics;

[Tool]
public partial class PlayerCamera : Camera2D
{
    [Export]
    public float DefaultZoom = 1.5f;

    [Export]
    public Node2D? Player;

    public override void _Process(double delta)
    {
        Debug.Assert(Player != null);

        Zoom = Vector2.One / Player.Scale * DefaultZoom;
    }
}
