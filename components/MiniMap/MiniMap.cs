public partial class MiniMap : CanvasLayer
{
    public const float MaxPlayerScale = 8f;
    public const float MinPlayerScale = 0.4f;
    public const float ZoomSpeed = 5f;
    public const float MiniMapScale = 0.05f;

    [Export]
    public required TileSet MinimapTileset;

    [Export]
    public WorldLayer[] Tilemaps = [];

    [Export]
    public required Control MinimapContainer;

    [Export]
    public required Node2D Player;

    public override void _Ready()
    {
        shadowLayers = Tilemaps
            .Select(layer =>
            {
                var shadowLayer = (TileMapLayer)layer.Duplicate();
                MinimapContainer.AddChild(shadowLayer);

                shadowLayer.Clear();
                shadowLayer.TileSet = (TileSet)MinimapTileset.Duplicate();
                shadowLayer.Scale = layer.TileSet.TileSize;

                return shadowLayer;
            })
            .ToArray();

        Visible = false;
    }

    public override void _Process(double delta)
    {
        if (!Visible) return;

        var zoom = Input.GetAxis(KeyMap.MapZoomOut, KeyMap.MapZoomIn);
        if (zoom == 0) return;

        Player.GlobalScale -= Vector2.One * zoom * ZoomSpeed * (float)delta;
        Player.GlobalScale = Player.GlobalScale.Clamp(MinPlayerScale, MaxPlayerScale);
        MinimapContainer.Scale = Vector2.One * MiniMapScale / Player.GlobalScale;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed(KeyMap.Map))
        {
            Visible = !Visible;

            if (Visible)
            {
                UpdateMinimap();
            }
        }
    }

    TileMapLayer[] shadowLayers = [];

    void UpdateMinimap()
    {
        shadowLayers
            .Zip(Tilemaps)
            .ToList()
            .ForEach(i =>
            {
                var (shadow, tilemap) = i;
                tilemap.GetUsedCells()
                    .Where(coords => tilemap.CellAt(coords).Seen)
                    .ToList()
                    .ForEach(coords => shadow.SetCell(coords, 0, Vector2I.Zero));
            });
    }
}
