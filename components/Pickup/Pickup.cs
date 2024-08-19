public partial class Pickup : Area2D
{
    public enum PickupType
    {
        Key
    }

    [Export]
    public PickupType Type;

    [Export]
    public string? Data;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    void OnAreaEntered(Node2D area)
    {
        if (area is not PickupCollector collector) return;

        collector.Collect(this);

        var tween = GetParent().CreateTween();
        tween.TweenProperty(GetParent(), "global_scale", Vector2.Zero, 0.1f);
        tween.Finished += () => GetParent().QueueFree();
    }
}
