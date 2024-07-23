using Godot;

namespace MyCraft.Scripts;

public partial class WorldManager : Node
{
   
    
    public override void _Ready()
    {

        TexturesAtlas.Initialize();
    }
}