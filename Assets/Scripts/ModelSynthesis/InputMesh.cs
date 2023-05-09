using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMesh
{
    public Texture2D Texture { get; private set; }
    public int TileSize { get; private set; }
    public List<Texture2D> Tiles { get; private set; }
    [Header("SharedData")]
    public SharedData sharedData;
    public InputMesh(Texture2D texture)
    {
        Texture = texture;
        TileSize = sharedData.TileSize;
        Tiles = ExtractTiles();
    }

    private List<Texture2D> ExtractTiles()
    {
        List<Texture2D> tiles = new List<Texture2D>();

        for (int y = 0; y < Texture.height; y += TileSize)
        {
            for (int x = 0; x < Texture.width; x += TileSize)
            {
                Texture2D tile = new Texture2D(TileSize, TileSize);
                tile.SetPixels(Texture.GetPixels(x, y, TileSize, TileSize));
                tile.Apply();
                tiles.Add(tile);
            }
        }

        return tiles;
    }
}