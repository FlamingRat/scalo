using System;
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
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;

        Debug.Assert(MessagePanel != null);
        MessagePanel.Visible = false;
    }

    long LastMessageTimestamp;

    async void OnAreaEntered(Node2D area)
    {
        Debug.Assert(Message != null && MessageBubble != null && Sprite != null && MessagePanel != null);

        if (area is not Sight) return;

        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.OutIn);
        tween.TweenProperty(Sprite, "position", Vector2.Up * 8, 0.1f);
        tween.TweenCallback(Callable.From(Fall));

        MessagePanel.Visible = true;
        MessageBubble.Text = "";

        var messageTimestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
        LastMessageTimestamp = messageTimestamp;
        var remainingMessage = Message;
        while (remainingMessage.Length > 0)
        {
            if (LastMessageTimestamp != messageTimestamp) break;

            MessageBubble.Text += remainingMessage[0];
            remainingMessage = remainingMessage[1..];
            await Task.Delay(20);
        }
    }

    void Fall()
    {
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.OutIn);
        tween.TweenProperty(Sprite, "position", Vector2.Zero, 0.1f);
    }

    void OnAreaExited(Node2D area)
    {
        Debug.Assert(MessageBubble != null && MessagePanel != null);

        if (area is not Sight) return;

        MessageBubble.Text = "";
        MessagePanel.Visible = false;
    }
}
