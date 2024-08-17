public partial class PlayerAnimationController : Node
{
    [Export]
    public CharacterBody2D Player;

    [Export]
    public Sprite2D Sprite;

    public override void _PhysicsProcess(double delta)
    {
        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        if (Mathf.Abs(Player.Velocity.X) > 0)
        {
            Sprite.Scale = Vector2.Down + Vector2.Right * Player.Velocity.Sign().X;
        }
    }
}
