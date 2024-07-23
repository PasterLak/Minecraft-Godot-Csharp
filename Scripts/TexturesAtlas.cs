using Godot;

namespace MyCraft.Scripts;

public class TexturesAtlas
{
    private static TexturesAtlas _instance;

    public static TexturesAtlas Instance
    {
        get
        {
            if (_instance != null) return _instance;
            return Initialize();
        }
        
    }

    private static Texture2D blocksTexture;
    private static Texture2D itemsTexture;

    public const int blockSize = 16;
    private static int blocksInWidth;
    private static float blockUVSize;

    public static Texture2D GetBlocksTexture()
    {
        if(_instance != null)
        return blocksTexture;

        Initialize();
        return blocksTexture;
    }

    public static TexturesAtlas Initialize()
    {

        _instance = new TexturesAtlas();
        
        LoadTextures();

        blocksInWidth = blocksTexture.GetWidth() / blockSize;
        blockUVSize = 1.0f / (float)blocksInWidth;

        if (blocksTexture.GetWidth() != blocksTexture.GetHeight())
        {
            GD.PrintErr("TexturesAtlas: Texture width and height should be the same!");
        }


        return _instance;
    }

    private static void LoadTextures()
    {
        blocksTexture = ResourcesLoader.LoadPNG("Textures/blocks.png");

        itemsTexture = ResourcesLoader.LoadPNG(
           "Textures/blocks.png");
    }

    /*public static Sprite GetItemSprite(Vector2Byte pos, TextureType type)
    {
        Texture2D texture = new Texture2D(blockSize, blockSize);

        for (int i = 0; i < texture.width; i++)
        {
            for (int p = 0; p < texture.height; p++)
            {
               if(type == TextureType.item)
                    texture.SetPixel(i, p,
                        itemsTexture.GetPixel(pos.x * blockSize + i, pos.y * blockSize + p));
               else
                    texture.SetPixel(i, p,
                        blocksTexture.GetPixel(pos.x * blockSize + i, pos.y * blockSize + p));
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, blockSize, blockSize), new Vector2()); 
    }*/

/*
    public static Texture2D GetTexture2D(Vector2Byte pos, TextureType type)
    {
        Texture2D texture = new Texture2D(blockSize, blockSize);

        for (int i = 0; i < texture.GetWidth(); i++)
        {
            for (int p = 0; p < texture.GetHeight(); p++)
            {
                texture.SetPixel(i, p,
                    type == TextureType.block
                        ? blocksTexture.GetPixel(pos.x * blockSize + i, pos.y * blockSize + p)
                        : itemsTexture.GetPixel(pos.x * blockSize + i, pos.y * blockSize + p));
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;
    }*/


    public static Vector2[] GetBlockTextureCoord(Vector2Byte pos)
    {
        if (pos.x >= blocksInWidth)
            pos.x = (byte)(blocksInWidth - 1);
        if (pos.y >= blocksInWidth)
            pos.y = (byte)(blocksInWidth - 1);
        if (pos.x < 0)
            pos.x = 0;
        if (pos.y < 0)
            pos.y = 0;

        Vector2[] uv = new Vector2[4];

        uv[0].X = blockUVSize * pos.x;
        uv[0].Y = blockUVSize * pos.y;

        uv[1].X = blockUVSize * pos.x;
        uv[1].Y = blockUVSize * pos.y + blockUVSize;

        uv[2].X = blockUVSize * pos.x + blockUVSize;
        uv[2].Y = blockUVSize * pos.y + blockUVSize;

        uv[3].X = blockUVSize * pos.x + blockUVSize;
        uv[3].Y = blockUVSize * pos.y;

        return uv;
    }

/*
    private Texture2D CreateTexture(int x, int y, FilterMode mode = FilterMode.Point)
    {
        Texture2D _texture = new Texture2D(x, y);

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                _texture.SetPixel(i, j, Random.ColorHSV());

            }
        }
        _texture.Apply();
        _texture.filterMode = mode;

        return _texture;
    }*/
}

public enum TextureType : byte
{
    Block,
    Item
}

