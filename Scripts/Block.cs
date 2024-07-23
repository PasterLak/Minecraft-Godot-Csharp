namespace MyCraft.Scripts;

public class Block
{
    public short Id;
    public byte Sides;
    public byte Durability;
    public Vector2Byte TextureCoord;

    public Block(Vector2Byte textureCoord)
    {
        TextureCoord = textureCoord;
    }
}

public enum BlockType : byte
{
    Rock,
    Lg
}