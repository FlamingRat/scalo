using System.Diagnostics;
using System.Threading.Tasks;

public partial class Sign : Node2D
{
    [Export(PropertyHint.MultilineText)]
    public string? Message;

    [Export]
    public AlertBox? AlertBox;

    [Export]
    public Sprite2D? Sprite;

    public override void _Ready()
    {
        Debug.Assert(AlertBox != null && Message != null);

        AlertBox.AreaEntered += AnimateHop;
        AlertBox.Message = Message;
    }

    void AnimateHop(Node2D body)
    {
        Debug.Assert(Sprite != null);

        var tween = CreateTween();
        tween.TweenProperty(Sprite, "position", Vector2.Up * 8, 0.1f);

        tween.Finished += async () =>
        {
            await Task.Delay(50);
            var tween = CreateTween();
            tween.TweenProperty(Sprite, "position", Vector2.Zero, 0.1f);
        };
    }
}
