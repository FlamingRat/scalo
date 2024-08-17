[Tool]
public partial class MiniMap : CanvasLayer
{
    public const float MaxMiniMapScale = 1f;
    public const float MinMiniMapScale = 0.1f;
    public const float ZoomSpeed = 0.1f;

    [Export]
    public TileMapLayer MainMap;

    [Export]
    public TileMapLayer Minimap;

    public override void _Ready()
    {
        if (!Engine.IsEditorHint())
        {
            Visible = false;

            Minimap.TileSet = MainMap.TileSet;
            Minimap.TileMapData = MainMap.TileMapData;
        }
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint()) EditorProcess();
        else Process(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed(KeyMap.Map) && !Engine.IsEditorHint())
        {
            Visible = !Visible;
        }
    }

    private void Process(double delta)
    {
        var zoom = Input.GetAxis(KeyMap.MapZoomOut, KeyMap.MapZoomIn);
        Minimap.Scale += Vector2.One * zoom * (float)delta;

        Minimap.Scale = Minimap.Scale.Clamp(MinMiniMapScale, MaxMiniMapScale);
    }

    private void EditorProcess()
    {
        if (Minimap != null && MainMap != null)
        {
            Minimap.TileSet = MainMap.TileSet;
            Minimap.TileMapData = MainMap.TileMapData;
        }
    }
}
