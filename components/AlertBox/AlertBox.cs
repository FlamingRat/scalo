using System;
using System.Diagnostics;
using System.Threading.Tasks;

[GlobalClass]
public partial class AlertBox : Area2D
{
    [Export(PropertyHint.MultilineText)]
    public string? Message;

    [Export]
    public RichTextLabel? MessageBubble;

    [Export]
    public Panel? MessagePanel;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;

        Debug.Assert(MessagePanel != null);
        MessagePanel.Visible = false;
    }

    void OnAreaEntered(Node2D area)
    {
        Debug.Assert(MessagePanel != null);

        if (area is not Sight) return;

        MessagePanel.Visible = true;

        RollMessage();
    }

    void OnAreaExited(Node2D area)
    {
        Debug.Assert(MessageBubble != null && MessagePanel != null);

        if (area is not Sight) return;

        MessageBubble.Text = "";
        MessagePanel.Visible = false;
    }

    long LastMessageTimestamp;

    async void RollMessage()
    {
        Debug.Assert(Message != null && MessageBubble != null);
        MessageBubble.Text = "";

        var messageTimestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
        LastMessageTimestamp = messageTimestamp;
        var remainingMessage = Message;
        var nextChar = remainingMessage[0];
        var advanceChar = () =>
        {
            MessageBubble.Text += nextChar;
            remainingMessage = remainingMessage[1..];

            if (remainingMessage.Length == 0) return;
            nextChar = remainingMessage[0];
        };

        while (remainingMessage.Length > 0)
        {
            if (LastMessageTimestamp != messageTimestamp) break;

            if (nextChar == ' ')
            {
                advanceChar();
            }

            if (nextChar == '[')
            {
                while (nextChar != ']')
                {
                    advanceChar();
                }
            }

            advanceChar();
            await Task.Delay(20);
        }
    }
}
