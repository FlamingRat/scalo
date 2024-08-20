using System.Diagnostics;

[GlobalClass]
public partial class SelfDestruct : Node
{
    [Export]
    public float Timer = 2f;

    [Export]
    public Node? Destroy;

    public override void _Process(double delta)
    {
        Debug.Assert(Destroy != null);

        Timer -= (float)delta;

        if (Timer <= 0) Destroy.QueueFree();
    }
}
