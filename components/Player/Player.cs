using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float ClimbSpeed = 150.0f;

    public bool Climbing { get { return climbing; } }
    public override void _PhysicsProcess(double delta)
    {
        ApplyXMovement();
        ApplyYMovement(delta);

        MoveAndSlide();
    }

    private void ApplyXMovement()
    {
        var velocity = Velocity;
        var direction = Math.Sign(Input.GetAxis(KeyMap.MoveL, KeyMap.MoveR));
        if (direction != 0f)
        {
            velocity.X = direction * Speed * GlobalScale.X;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
    }

    bool climbing = false;

    private void ApplyYMovement(double delta)
    {
        var velocity = Velocity;
        climbing = Input.IsActionPressed(KeyMap.ClimbUp) && IsOnWall();

        if (Climbing)
        {
            velocity.Y = -ClimbSpeed * Scale.Y;
        }
        else if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta * GlobalScale.X;
        }
        else
        {
            velocity.Y = 0f;
        }

        Velocity = velocity;
    }
}
