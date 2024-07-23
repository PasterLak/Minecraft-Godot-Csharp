using Godot;

public class ResourcesLoader  
{
	

	public static string GetApplicationPath()
	{
		string path = "";
/*
		path = Application.dataPath;

		if(Application.isEditor)
			path = path.Replace ("/Assets", "");
		if(!Application.isEditor)
			path = path.Replace ("/" + Application.productName + "_Data", "");*/
		return path;

	}


	public static Texture2D LoadPNG(string filePath) // For example: Textures/blocks.png
	{
		Texture2D texture = new Texture2D();
	
		string texturePath = "res://" + filePath;
        
		
		texture = ResourceLoader.Load<Texture2D>(texturePath);

		if (texture == null)
		{
			GD.PrintErr("Failed to load texture at path: " + texturePath);
		}
		else
		{
			GD.Print("Texture successfully loaded!");
		}

		return texture;
	}



}
