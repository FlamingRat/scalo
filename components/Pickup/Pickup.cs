public partial class Pickup : Area2D
{
    public enum ItemType
    {
        Key,
        Lock,
    }

    public enum InteractionID
    {
        Test,
        FakeLock,
        TutorialLock,
    }

    [Export]
    public ItemType Type;

    [Export]
    public InteractionID Interaction;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    void OnAreaEntered(Node2D area)
    {
        if (area is not PickupCollector collector) return;

        if (!collector.Collect(this)) return;

        var tween = GetParent().CreateTween();
        tween.TweenProperty(GetParent(), "global_scale", Vector2.Zero, 0.1f);
        tween.Finished += () => GetParent().QueueFree();
    }
}
