public class Tile
{
    public enum TileType { Ground, Water, Forest, Mountain, Brick }
    public TileType type;
    public int rotation;

    public Tile(TileType type, int rotation)
    {
        this.type = type;
        this.rotation = rotation;
    }
}



