using System.Linq;

[GlobalClass]
public partial class PickupCollector : Area2D
{
    public bool Collect(Pickup pickup)
    {
        if (!CanPickup(pickup)) return false;

        collectedItems = collectedItems.Append(new Item(pickup.Type, pickup.Interaction)).ToArray();
        return true;
    }

    bool CanPickup(Pickup pickup)
    {
        if (pickup.Type == Pickup.ItemType.Lock)
        {
            var lockId = pickup.Interaction;
            GD.Print(lockId);
            GD.Print(collectedItems);
            return collectedItems.Any(item => item.Type == Pickup.ItemType.Key && item.Data == lockId);
        }

        return true;
    }

    record Item(Pickup.ItemType Type, Pickup.InteractionID Data);
    Item[] collectedItems = [];
}
