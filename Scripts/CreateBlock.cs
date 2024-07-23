using Godot;
using System;
using System.Collections.Generic;
using MyCraft.Scripts;


public struct SidesData
{
    public byte Back;
    public byte Left;
    public byte Forward;
    public byte Right;
    public byte Up;
    public byte Down;
}

public partial class CreateBlock : MeshInstance3D
{

    [Export]
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
    

    private void CreateData()
    {

        _vertices = new List<Vector3>();
        _uvs = new List<Vector2>();
        _indices = new List<int>();

        texture = TexturesAtlas.GetBlocksTexture();
    }

    private void CreateBlock2()
    {
        SidesData data = new SidesData();
    
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y< 8; y++)
            {
                for (int z = 0; z < 8; z++)
                {
                    Vector2Byte blockCoord = new Vector2Byte(4, 3);

                    if (y == 7) blockCoord = new Vector2Byte(3,3);
                    
                    if (y == 0) blockCoord = new Vector2Byte(3,0);
                    
                    CreateSides(new Vector3I(x,y,z),ref data, ref blockCoord);
                }
            }
        }
    }

    private void CreateSides(Vector3I pos, ref SidesData sidesData, ref Vector2Byte blockCoord)
    {
        
        CreateSide(Vector3I.Back, pos,1f, ref blockCoord);
        CreateSide(Vector3I.Left, pos,1f, ref blockCoord);
        CreateSide(Vector3I.Forward, pos,1f, ref blockCoord);
        CreateSide(Vector3I.Right, pos,1f, ref blockCoord);
        CreateSide(Vector3I.Up, pos,1f, ref blockCoord);
        CreateSide(Vector3I.Down,pos, 1f, ref blockCoord);
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
        CreateBlock2();

        material.AlbedoTexture = texture;
/*
        material = new StandardMaterial3D
        {
            AlbedoColor = color,
            AlbedoTexture = texture,
            TextureFilter = BaseMaterial3D.TextureFilterEnum.Nearest
        };*/

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

        CreateCollisionShape();
      
        GD.Print("Ready!");
    }
    
    private void CreateCollisionShape()
    {

        // Create a shape that matches the mesh
        var concavePolygonShape3D = new ConvexPolygonShape3D();
        concavePolygonShape3D.Points = _vertices.ToArray();
        shape.Shape = concavePolygonShape3D;
    }
    
    
}