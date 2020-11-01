using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneLoadCard : MonoBehaviour
{
    public TextMeshProUGUI date;
    public RawImage image;
    public Button delete;
    public Button load;
    public void Set(ZenSceneData data)
    {
        date.text = data.dateTime.ToString("yyyy/MM/dd");
        try
        {
            image.texture = ImageImporter.loadImage(new Vector2(1080, 1080), data.imagePath);
        }
        catch (System.Exception)
        {
            Debug.Log("no image found");
        }
        
    }
}
