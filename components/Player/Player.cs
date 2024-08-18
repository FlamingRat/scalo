public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float ClimbSpeed = 150.0f;
    public override void _PhysicsProcess(double delta)
    {
        ApplyXMovement();
        ApplyYMovement(delta);

        MoveAndSlide();
    }

    private void ApplyXMovement()
    {
        var velocity = Velocity;
        var direction = Input.GetAxis(KeyMap.MoveL, KeyMap.MoveR);
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

    private void ApplyYMovement(double delta)
    {
        var velocity = Velocity;
        var direction = Input.GetAxis(KeyMap.ClimbUp, KeyMap.ClimbDown);
        if (direction != 0f && IsOnWall())
        {
            velocity.Y = direction * ClimbSpeed * Scale.Y;
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
