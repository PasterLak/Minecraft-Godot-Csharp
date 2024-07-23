using Godot;

namespace MyCraft.Scripts;

public partial class BuildingAbility : Node3D
{
    [Export]
    private Camera3D _camera;

  
    private Node3D select;
    [Export]
    private PackedScene selectPrefab;
    
    private const string SelectorPrefab = "res://Prefabs/selector.tscn";


    public override void _EnterTree()
    {
       
    }

    public override void _Ready()
    {
            select = selectPrefab.Instantiate<Node3D>();
  
          Node root = GetTree().Root.GetChild(0);
          
       

          root.CallDeferred("add_child", select);
          
         
          if(select == null) GD.Print("No select");
        return;
        var chunkPrefab = (PackedScene)ResourceLoader.Load(SelectorPrefab);

        if (chunkPrefab == null)
        {
            GD.PrintErr("Could not load prefab at path: " + SelectorPrefab);
            return;
        }

        var prefabInstance = chunkPrefab.Instantiate();

        //select = prefabInstance as Node3D;
        
        GetTree().Root.AddChild(prefabInstance);
    }
    
  
    public override void _Process(double delta)
    {
      
        var v = GetViewport().GetVisibleRect();
        Vector2 screenCenter = v.Size / 2;

        
        Vector3 from = _camera.ProjectRayOrigin(screenCenter);
        Vector3 to = from + _camera.ProjectRayNormal(screenCenter) * 50; 

      
        var spaceState = GetWorld3D().DirectSpaceState;
        
        PhysicsRayQueryParameters3D ray = PhysicsRayQueryParameters3D.Create(from,to);
        var result = spaceState.IntersectRay(ray);

        if (result.Count > 0)
        {
          
            Vector3 intersectionPoint = (Vector3)result["position"];
        
            intersectionPoint -= new Vector3(0, 0.5f, 0);
            var rounded =  intersectionPoint.Floor();

            select.Position = rounded;
         
        }
        else
        {
            select.Position = new Vector3(0,-1000,0);
        }
    }
}