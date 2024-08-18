public partial class Sight : Area2D
{
    public override void _Process(double delta)
    {
        foreach (var body in GetOverlappingBodies().ToList())
        {
            if (!(body is WorldLayer layer)) continue;

            var tileSize = layer.TileSet.TileSize;
            if (tileSize.X < 16)
            {
                layer.TilesetSeen = true;
                continue;
            }


            var centerTile = (Vector2I)(GlobalPosition / tileSize).Round();
            var range = (CircleShape2D)GetChild<CollisionShape2D>(0).Shape;
            var tileRange = (int)(new Vector2(range.Radius, range.Radius) / tileSize).X;

            // @todo remove nested loops eww
            for (var y = centerTile.Y - tileRange; y < centerTile.Y + tileRange; y++)
            {
                for (var x = centerTile.X - tileRange; x < centerTile.X + tileRange; x++)
                {
                    var cell = layer.CellAt(x, y);
                    if (cell.Seen) continue;

                    cell.Seen = true;
                }
            }
        }
    }
}
