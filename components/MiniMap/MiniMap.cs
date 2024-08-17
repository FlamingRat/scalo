[Tool]
public partial class MiniMap : CanvasLayer
{
    public const float MaxPlayerScale = 1f;
    public const float MinPlayerScale = 0.1f;
    public const float ZoomSpeed = 1f;

    [Export]
    public TileMapLayer World;

    [Export]
    public TileMapLayer Minimap;

    [Export]
    public Node2D Player;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;

        Visible = false;

        Minimap.TileSet = World.TileSet;
        Minimap.TileMapData = World.TileMapData;
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint()) EditorProcess();
        else Process((float)delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed(KeyMap.Map) && !Engine.IsEditorHint())
        {
            Visible = !Visible;
        }
    }

    private void Process(float delta)
    {
        Minimap.GlobalScale = Vector2.One / Player.GlobalScale * 0.4f;

        if (!Visible) return;

        var zoom = Input.GetAxis(KeyMap.MapZoomOut, KeyMap.MapZoomIn);
        if (zoom == 0) return;

        Player.GlobalScale -= Vector2.One * zoom * ZoomSpeed * delta;

        Player.GlobalScale = Player.GlobalScale.Clamp(MinPlayerScale, MaxPlayerScale);
    }

    private void EditorProcess()
    {
        if (Minimap == null && World == null) return;

        Minimap.TileSet = World.TileSet;
        Minimap.TileMapData = World.TileMapData;
    }
}
