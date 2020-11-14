using Godot;
using System;

public class TileSetMaker : Node
{
    public int TileWidth = 128;
    public int TileHeight = 128;
    Sprite sprite;
    Texture texture;

    public override void _Ready()
    {
        sprite = (Sprite)GetNode("Sprite");
        texture = sprite.Texture;

        var width = texture.GetWidth() / TileWidth;
        var height = texture.GetHeight() / TileHeight;
        var ts = new TileSet();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var region = new Rect2(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
                var id = x + y * 10;
                ts.CreateTile(id);
                ts.TileSetTexture(id, texture);
                ts.TileSetRegion(id, region);
                ResourceSaver.Save("res://terrain/terrain_tiles.tres",ts);
            }
        }

        base._Ready();
    }
}
