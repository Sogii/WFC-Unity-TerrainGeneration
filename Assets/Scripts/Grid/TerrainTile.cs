public class TerrainTile
{
    public enum TileType { Ground, Water, Forest, Mountain, Brick }
    public SharedData.TerrainType TerrainType;
    public TileType type;
    public int rotation;

    public TerrainTile(TileType type, int rotation)
    {
        this.type = type;
        this.rotation = rotation;
    }
}



