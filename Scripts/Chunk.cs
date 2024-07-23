using Godot;
using System;
using System.Collections.Generic;
using MyCraft.Scripts;



public partial class Chunk : MeshInstance3D
{

    public const byte ChunkHeight = 16;
    public const byte ChunkXZ = 8;

   
    private Texture2D texture;

    [Export] private CollisionShape3D shape;
    [Export]
    private Color color = new Color(0.9f, 0.1f, 0.1f);
    
    private ArrayMesh tmpMesh = new ArrayMesh();

    [Export]
    private StandardMaterial3D material;

    private List<Vector3> _vertices;
    private List<Vector2> _uvs;
    private List<int> _indices;

    private Block[,,] _blocks;

    private void CreateData()
    {

        _vertices = new List<Vector3>();
        _uvs = new List<Vector2>();
        _indices = new List<int>();

        _blocks = new Block[ChunkXZ, ChunkHeight, ChunkXZ];
        
        texture = TexturesAtlas.GetBlocksTexture();
    }

    private void CreateSidesData()
    {
        for (int x = 0; x < ChunkXZ; x++)
        {
            for (int y = 0; y< ChunkHeight; y++)
            {
                for (int z = 0; z < ChunkXZ; z++)
                {
                    
                    if(_blocks[x, y, z] == null) continue;

                    if (x > 0 && x < ChunkXZ - 1)
                    {
                        if (_blocks[x - 1, y, z] == null) _blocks[x, y, z].Sides += 2;
                        if (_blocks[x + 1, y, z] == null) _blocks[x, y, z].Sides += 8;
                    }

                    if (x == 0)
                    {
                        if (_blocks[x + 1, y, z] == null) _blocks[x, y, z].Sides += 8;
                        _blocks[x, y, z].Sides += 2;
                    }

                    if (x == ChunkXZ - 1)
                    {
                        if (_blocks[x - 1, y, z] == null) _blocks[x, y, z].Sides += 2;
                        _blocks[x, y, z].Sides += 8;
                    }
                    
                    if (y > 0 && y < ChunkHeight - 1)
                    {
                        if (_blocks[x, y-1, z] == null) _blocks[x, y, z].Sides += 32;
                        if (_blocks[x, y+1, z] == null) _blocks[x, y, z].Sides += 16;
                    }

                    if (y == 0)
                    {
                        if (_blocks[x, y+1, z] == null) _blocks[x, y, z].Sides += 16;
                        _blocks[x, y, z].Sides += 32;
                    }

                    if (y == ChunkHeight - 1)
                    {
                        if (_blocks[x, y-1, z] == null) _blocks[x, y, z].Sides += 32;
                        _blocks[x, y, z].Sides += 16;
                    }
                    if (z > 0 && z < ChunkXZ - 1)
                    {
                        if (_blocks[x, y, z-1] == null) _blocks[x, y, z].Sides += 4;
                        if (_blocks[x, y, z+1] == null) _blocks[x, y, z].Sides += 1;
                    }

                    if (z == 0)
                    {
                        if (_blocks[x, y, z+1] == null) _blocks[x, y, z].Sides += 1;
                        _blocks[x, y, z].Sides += 4;
                    }

                    if (z == ChunkXZ - 1)
                    {
                        if (_blocks[x, y, z-1] == null) _blocks[x, y, z].Sides += 4;
                        _blocks[x, y, z].Sides += 1;
                    }
                }
            }
        }
    }

    private void CreateBlocksData()
    {
        for (int x = 0; x < ChunkXZ; x++)
        {
            for (int y = 0; y< ChunkHeight; y++)
            {
                for (int z = 0; z < ChunkXZ; z++)
                {
                    Vector2Byte blockCoord = new Vector2Byte(4, 3);

                    if (y == ChunkHeight - 1) blockCoord = new Vector2Byte(3,3);
                    
                    if (y == 0) blockCoord = new Vector2Byte(3,0);
                    
                    if(y < 10 && y > 0)
                        blockCoord = new Vector2Byte(4,0);

                    _blocks[x, y, z] = new Block(blockCoord);
                }
            }
        }
    }

    private void CreateBlocks()
    {
      
        for (int x = 0; x < ChunkXZ; x++)
        {
            for (int y = 0; y< ChunkHeight; y++)
            {
                for (int z = 0; z < ChunkXZ; z++)
                {

                    if(_blocks[x, y, z] == null) continue;
                    
                    CreateSides(new Vector3I(x,y,z), _blocks[x,y,z].Sides, ref _blocks[x, y, z].TextureCoord);
                }
            }
        }
    }

    private void CreateSides(Vector3I pos, byte sides, ref Vector2Byte blockCoord)
    {
      
        if (sides - 32 >= 0)
        {
            sides -= 32;
            CreateSide(Vector3I.Down,pos, 1f, ref blockCoord);
        }
        if (sides - 16 >= 0)
        {
            sides -= 16;
            CreateSide(Vector3I.Up,pos, 1f, ref blockCoord);
        }
        if (sides - 8 >= 0)
        {
            sides -= 8;
            CreateSide(Vector3I.Right,pos, 1f, ref blockCoord);
        }
        if (sides - 4 >= 0)
        {
            sides -= 4;
            CreateSide(Vector3I.Forward,pos, 1f, ref blockCoord);
        }
        if (sides - 2 >= 0)
        {
            sides -= 2;
            CreateSide(Vector3I.Left,pos, 1f, ref blockCoord);
        }
        if (sides - 1 >= 0)
        {
            sides -= 1;
            CreateSide(Vector3I.Back,pos, 1f, ref blockCoord);
        }

    }

    private void AddUVs(ref Vector2Byte blockCoord)
    {
        _uvs.AddRange(TexturesAtlas.GetBlockTextureCoord(blockCoord));

    }

    private void CreateSide(Vector3I direction,Vector3I pos, float size, ref Vector2Byte blockCoord)
    {
        
        int startIndex = _vertices.Count;

        if (direction == Vector3I.Back)
        {
            _vertices.Add(new Vector3(0, 0, size)+ pos);
            _vertices.Add(new Vector3(0, size, size) + pos);
            _vertices.Add(new Vector3(size, size, size) + pos);
            _vertices.Add(new Vector3(size, 0, size) + pos);
        }
        if (direction == Vector3I.Left)
        {
            _vertices.Add(new Vector3(0, 0, 0) + pos);
            _vertices.Add(new Vector3(0, size, 0) + pos);
            _vertices.Add(new Vector3(0, size, size) + pos);
            _vertices.Add(new Vector3(0, 0, size) + pos);
        }
        if (direction == Vector3I.Forward)
        {
            _vertices.Add(new Vector3(size, 0, 0)+ pos);
            _vertices.Add(new Vector3(size, size, 0)+ pos);
            _vertices.Add(new Vector3(0, size, 0)+ pos);
            _vertices.Add(new Vector3(0, 0, 0)+ pos);
        }
        if (direction == Vector3I.Right)
        {
            _vertices.Add(new Vector3(size, 0, size)+ pos);
            _vertices.Add(new Vector3(size, size, size)+ pos);
            _vertices.Add(new Vector3(size, size, 0)+ pos);
            _vertices.Add(new Vector3(size, 0, 0)+ pos);
        }
        if (direction == Vector3I.Up)
        {
            _vertices.Add(new Vector3(0, size, size)+ pos);
            _vertices.Add(new Vector3(0, size, 0)+ pos);
            _vertices.Add(new Vector3(size, size, 0)+ pos);
            _vertices.Add(new Vector3(size, size, size)+ pos);
        }
        if (direction == Vector3I.Down)
        {
           
            _vertices.Add(new Vector3(0, 0, 0)+ pos); 
            _vertices.Add(new Vector3(0, 0, size)+ pos);
            _vertices.Add(new Vector3(size, 0, size)+ pos);
            _vertices.Add(new Vector3(size, 0, 0)+ pos);
            
        }
        
        AddUVs(ref blockCoord);
            
        _indices.Add(startIndex + 0);
        _indices.Add(startIndex + 1);
        _indices.Add(startIndex + 2);
        _indices.Add(startIndex + 2);
        _indices.Add(startIndex + 3);
        _indices.Add(startIndex + 0);
    }
    
    
    public override void _Ready()
    {
        CreateData();
        CreateBlocksData();
        CreateSidesData();
        CreateBlocks();

        material.AlbedoTexture = texture;

        var st = new SurfaceTool();

        st.Begin(Mesh.PrimitiveType.Triangles);
        st.SetMaterial(material);

        for (int i = 0; i < _vertices.Count; i++)
        {
            st.SetColor(color);
            st.SetUV(_uvs[i]);
            st.AddVertex(_vertices[i]);
        }

        for (int i = 0; i < _indices.Count; i++)
        {
            st.AddIndex(_indices[i]);
        }

        st.Commit(tmpMesh);

        base.Mesh = tmpMesh;

        //CreateCollisionShape();
      
        GD.Print("Chunk was created!");
    }
    
    private void CreateCollisionShape()
    {
        // Create a shape that matches the mesh
        var concavePolygonShape3D = new ConvexPolygonShape3D();
        concavePolygonShape3D.Points = _vertices.ToArray();
        shape.Shape = concavePolygonShape3D;
    }
    
    
}