using System.Linq;

public partial class MiniMap : CanvasLayer
{
    public const float MaxPlayerScale = 8f;
    public const float MinPlayerScale = 0.4f;
    public const float ZoomSpeed = 5f;
    public const float MiniMapScale = 0.05f;

    [Export]
    public TileMapLayer[] Tilemaps;

    [Export]
    public Control MinimapContainer;

    [Export]
    public Node2D Player;

    [Export]
    public Sprite2D PlayerMarker;

    public override void _Ready()
    {
        foreach (var layer in Tilemaps)
        {
            var copy = (TileMapLayer)layer.Duplicate();
            MinimapContainer.AddChild(copy);

            copy.TileSet = (TileSet)copy.TileSet.Duplicate();
            copy.TileSet.RemovePhysicsLayer(0);

            foreach (var coords in copy.GetUsedCells())
            {
                copy.SetCell(coords, 1, new Vector2I(0, 0));
            }
        }

        Visible = false;

        UpdateMinimap();
    }

    public override void _Process(double delta)
    {
        if (!Visible) return;

        UpdateMinimap();
        UpdatePlayerMarker();

        var zoom = Input.GetAxis(KeyMap.MapZoomOut, KeyMap.MapZoomIn);
        if (zoom == 0) return;

        Player.GlobalScale -= Vector2.One * zoom * ZoomSpeed * (float)delta;
        Player.GlobalScale = Player.GlobalScale.Clamp(MinPlayerScale, MaxPlayerScale);
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed(KeyMap.Map) && !Engine.IsEditorHint())
        {
            Visible = !Visible;
        }
    }

    private void UpdateMinimap()
    {
        MinimapContainer.Scale = Vector2.One * MiniMapScale / Player.GlobalScale;
    }

    private void UpdatePlayerMarker()
    {
        if (PlayerMarker == null || Player == null)
        {
            GD.PrintErr("PlayerMarker not found");
            return;
        }

        Vector2 miniMapPosition = Player.GlobalPosition / Player.GlobalScale * MiniMapScale;

        MinimapContainer.Position = -miniMapPosition;
    }
}
