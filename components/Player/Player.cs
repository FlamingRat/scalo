public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public override void _PhysicsProcess(double delta)
    {
        ApplyGravity(delta);
        ApplyXMovement();

        MoveAndSlide();
    }

    private void ApplyGravity(double delta)
    {
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta * GlobalScale.X;
        }
    }

    private void ApplyXMovement()
    {
        var velocity = Velocity;
        var direction = Input.GetAxis(KeyMap.MoveL, KeyMap.MoveR);
        if (direction != 0f)
        {
            velocity.X = direction * Speed * Scale.X;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
    }
}
