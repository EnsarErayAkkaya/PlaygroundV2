using UnityEngine;

public class ImageImporter
{
    public Vector2 size = new Vector2(1080, 1080); // image size
 
    public static Texture2D loadImage(Vector2 size, string filePath) 
    {     
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.LoadImage(bytes);

        return texture;
    }
}
