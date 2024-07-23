using Godot;
using System;

public partial class CreateBlock : MeshInstance3D
{

    [Export]
    private Texture2D texture;
    [Export]
    private Color color = new Color(0.9f, 0.1f, 0.1f);
    
    private ArrayMesh tmpMesh = new ArrayMesh();
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] indices;
    private StandardMaterial3D material;


    private void CreateData()
    {
        vertices = new Vector3[8];
        uvs = new Vector2[8];
        indices = new int[12];
    }

    private void CreateSide(Vector3I direction)
    {
        
    }
    
    
    public override void _Ready()
    {
        // Определение вершин для квадрата
        vertices = new Vector3[8];
        uvs = new Vector2[8];
        indices = new int[12];

        // Координаты вершин квадрата
        vertices[0] = new Vector3(0, 0, 0); // top left
        vertices[1] = new Vector3(1, 0, 0); // top right
        vertices[2] = new Vector3(1, 0, 1); // bottom right
        vertices[3] = new Vector3(0, 0, 1); // bottom left
        
        vertices[4] = new Vector3(0, 1, 0); // top left
        vertices[5] = new Vector3(1, 1, 0); // top right
        vertices[6] = new Vector3(1, 1, 1); // bottom right
        vertices[7] = new Vector3(0, 1, 1); // bottom left

        // Соответствующие UV координаты для каждой вершины
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(0, 1);
        
        uvs[4] = new Vector2(0, 0);
        uvs[5] = new Vector2(1, 0);
        uvs[6] = new Vector2(1, 1);
        uvs[7] = new Vector2(0, 1);

        // Индексы для двух треугольников
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        indices[3] = 0;
        indices[4] = 2;
        indices[5] = 3;
        
        indices[6] = 4;
        indices[7] = 5;
        indices[8] = 6;
        indices[9] = 4;
        indices[10] = 6;
        indices[11] = 7;

        material = new StandardMaterial3D();
        material.AlbedoColor = color;
        
        
        material.AlbedoTexture = texture;
        material.TextureFilter = BaseMaterial3D.TextureFilterEnum.NearestWithMipmaps;
        material.TextureFilter = BaseMaterial3D.TextureFilterEnum.Nearest;
        
        
        var st = new SurfaceTool();

        st.Begin(Mesh.PrimitiveType.Triangles);
        st.SetMaterial(material);

        // Добавление вершин и UV координат
        for (int i = 0; i < vertices.Length; i++)
        {
            st.SetColor(color);
            st.SetUV(uvs[i]);
            st.AddVertex(vertices[i]);
        }

        // Добавление индексов
        for (int i = 0; i < indices.Length; i++)
        {
            st.AddIndex(indices[i]);
        }

        st.Commit(tmpMesh);

        base.Mesh = tmpMesh;

        GD.Print("Ready!");
    }
}