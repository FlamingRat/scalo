using System.Linq;

[GlobalClass]
public partial class PickupCollector : Area2D
{
    public void Collect(Pickup pickup) => collectedItems.Append(new Item(pickup.Type, pickup.Data));

    record Item(Pickup.PickupType Type, string? Data);
    Item[] collectedItems = [];
}
