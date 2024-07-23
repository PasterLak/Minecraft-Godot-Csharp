using Godot;

namespace MyCraft.Scripts;

public partial class Sector : Node3D
{

    public const byte SectorSize = 8;
    public const byte SectorSizeBlocks = Chunk.ChunkXZ * SectorSize;
    
    private const string ChunkPrefab = "res://Prefabs/Chunk.tscn";

    private Chunk[,] _chunks = new Chunk[SectorSize, SectorSize];

    public bool ChunkExists(Vector2I localPosition)
    {
        if (localPosition.X < 0) return false;
        if (localPosition.Y < 0) return false;
        
        if (localPosition.X > SectorSize-1) return false;
        if (localPosition.Y > SectorSize-1) return false;

        return _chunks[localPosition.X, localPosition.Y] != null;
    }
    
    private void InstantiateChunk(Vector2I localPosition)
    {
        var chunkPrefab = (PackedScene)ResourceLoader.Load(ChunkPrefab);

        if (chunkPrefab == null)
        {
            GD.PrintErr("Could not load prefab at path: " + ChunkPrefab);
            return;
        }

        var prefabInstance = chunkPrefab.Instantiate();
        
        AddChild(prefabInstance);

        var mesh = prefabInstance.GetNode<MeshInstance3D>("Mesh");
        
        _chunks[localPosition.X, localPosition.Y] = mesh.GetNode<Chunk>(".");
      
    }

    private void GenerateChunks()
    {
        for (int x = 0; x < SectorSize; x++)
        {
            for (int y = 0; y < SectorSize; y++)
            {
                _chunks[x, y].Generate(new Vector3I(x,0,y) * Chunk.ChunkXZ, new Vector2I(x,y),this);
            }
        }
    }

    private void PrepareChunks()
    {
        for (int x = 0; x < SectorSize; x++)
        {
            for (int y = 0; y < SectorSize; y++)
            {
                InstantiateChunk(new Vector2I(x,y));
            }
        }
    }

    public override void _Ready()
    {
        PrepareChunks();
        GenerateChunks();
    }
}