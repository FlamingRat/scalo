using System;
using System.Diagnostics;
using System.Linq;

public partial class MiniMap : CanvasLayer
{
    public const float MaxPlayerScale = 8f;
    public const float MinPlayerScale = 0.4f;
    public const float ZoomSpeed = 5f;
    public const float MiniMapScale = 0.05f;

    [Export]
    public TileMapLayer[]? Tilemaps;

    [Export]
    public Control? MinimapContainer;

    [Export]
    public Node2D? Player;

    [Export]
    public Sprite2D? PlayerMarker;

    public override void _Ready()
    {
        Debug.Assert(Tilemaps != null && MinimapContainer != null);

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
        Debug.Assert(Player != null);

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
            GetTree().Paused = Visible;
        }
    }

    private void UpdateMinimap()
    {
        Debug.Assert(MinimapContainer != null && Player != null);

        MinimapContainer.Scale = Vector2.One * MiniMapScale / Player.GlobalScale;
    }

    private void UpdatePlayerMarker()
    {
        Debug.Assert(MinimapContainer != null && Player != null);

        Vector2 miniMapPosition = Player.GlobalPosition / Player.GlobalScale * MiniMapScale;

        MinimapContainer.Position = -miniMapPosition;
    }
}
