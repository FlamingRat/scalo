using System.Diagnostics;
using System.Threading.Tasks;

public partial class Sign : Area2D
{
    [Export(PropertyHint.MultilineText)]
    public string? Message;

    [Export]
    public Label? MessageBubble;

    [Export]
    public Panel? MessagePanel;

    [Export]
    public Sprite2D? Sprite;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;

        Debug.Assert(MessagePanel != null);
        MessagePanel.Visible = false;
    }

    async void OnBodyEntered(Node2D body)
    {
        Debug.Assert(Message != null && MessageBubble != null && Sprite != null && MessagePanel != null);

        if (body is not Player) return;

        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.OutIn);
        tween.TweenProperty(Sprite, "position", Vector2.Up * 8, 0.1f);
        tween.TweenCallback(Callable.From(Fall));

        MessagePanel.Visible = true;
        MessageBubble.Text = "";

        var remainingMessage = Message;
        while (remainingMessage.Length > 0)
        {
            MessageBubble.Text += remainingMessage[0];
            remainingMessage = remainingMessage[1..];
            await Task.Delay(40);
        }
    }

    void Fall()
    {
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.OutIn);
        tween.TweenProperty(Sprite, "position", Vector2.Zero, 0.1f);
    }

    void OnBodyExited(Node2D body)
    {
        Debug.Assert(MessageBubble != null && MessagePanel != null);

        if (body is not Player) return;

        MessageBubble.Text = "";
        MessagePanel.Visible = false;
    }
}
