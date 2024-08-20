using System.Diagnostics;

public partial class Checkpoint : Area2D
{
    const int SpriteFrameIfVisited = 66;

    [Export]
    public bool Visited = false;

    [Export]
    public Sprite2D? Sprite;

    public void Visit()
    {
        Debug.Assert(Sprite != null);

        Visited = true;
        Sprite.Modulate = new Color(1, 1, 1, 1);
        Sprite.Frame = SpriteFrameIfVisited;
    }
}
