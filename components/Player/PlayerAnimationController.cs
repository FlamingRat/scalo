using System.Diagnostics;

public partial class PlayerAnimationController : Node
{
    const int PhysicsFps = 60;

    static class PlayerSpriteFrame
    {
        public const int Idle = 0;
        public const int Airborne = 1;

        public static int WalkAnimation(int current)
        {
            return current == Idle ? Airborne : Idle;
        }
    }

    [Export]
    public Player? Player;

    [Export]
    public Sprite2D? Sprite;

    public override void _PhysicsProcess(double delta)
    {
        UpdateFacingDirection();
        UpdateWalkingAnimation();
    }

    void UpdateFacingDirection()
    {
        Debug.Assert(Player != null && Sprite != null);

        if (Mathf.Abs(Player.Velocity.X) > 0)
        {
            Sprite.Scale = Vector2.Down + Vector2.Right * Player.Velocity.Sign().X;
        }
    }


    void UpdateWalkingAnimation()
    {
        Debug.Assert(Player != null && Sprite != null);

        if (Player.Velocity.Y != 0 && !Player.Climbing)
        {
            Sprite.GlobalRotation = 0;
            Sprite.Frame = PlayerSpriteFrame.Airborne;
            return;
        }

        if (Player.Climbing)
        {
            Sprite.GlobalRotation = (Input.IsActionPressed(KeyMap.MoveL) ? 0.5f : -0.5f) * Mathf.Pi;
        }

        if (Player.Velocity.IsZeroApprox())
        {
            Sprite.Frame = PlayerSpriteFrame.Idle;
            return;
        }

        if (Engine.GetPhysicsFrames() % (PhysicsFps / 5) == 0)
        {
            Sprite.Frame = PlayerSpriteFrame.WalkAnimation(Sprite.Frame);
        }
    }
}
