[Tool]
public partial class MiniMap : CanvasLayer
{
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
        if (Engine.IsEditorHint() && Minimap != null && MainMap != null)
        {
            Minimap.TileSet = MainMap.TileSet;
            Minimap.TileMapData = MainMap.TileMapData;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed(KeyMap.Map) && !Engine.IsEditorHint())
        {
            Visible = !Visible;
        }
    }
}
