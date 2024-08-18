using System.Collections.Generic;

public partial class WorldLayer : TileMapLayer
{
    public bool TilesetSeen = false;

    public Dictionary<string, CellData> Cells { get { return cellData; } }

    public record CellData
    {
        public bool Seen { get { return seen; } set { seen = value; } }

        bool seen;
    };

    public CellData CellAt(string coords)
    {
        if (!cellData.ContainsKey(coords))
        {
            cellData[coords] = new CellData { Seen = false };
        }

        return cellData[coords];
    }

    public CellData CellAt(Vector2I coords)
    {
        return CellAt(coords.ToString());
    }

    public CellData CellAt(int x, int y)
    {
        return CellAt(new Vector2I(x, y));
    }

    Dictionary<string, CellData> cellData = new();
}
