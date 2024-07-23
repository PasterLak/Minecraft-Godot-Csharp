namespace MyCraft.Scripts;

public class Block
{
    public short Id;
    public byte Sides;
    public byte Durability;
}

public enum BlockType : byte
{
    Rock,
    Lg
}