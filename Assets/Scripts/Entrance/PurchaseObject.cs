using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class PurchaseObject : MonoBehaviour
{
    public Button buy;
    public TextMeshProUGUI cost, title;
    public Image image;

    public void Set(PurchaseItem p)
    {
        cost.text = p.cost.ToString("C", new System.Globalization.CultureInfo("en-us"));
        title.text = p.title;
        image.sprite = p.image;

        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            buy.interactable = false; // there is no buying system for pc right now
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if(p.isRail)
            {
                if(SaveAndLoadGameData.instance.savedData.playerRails.Any(s => s == p.railType))
                {
                    buy.interactable = false;
                }
            }
            else if(p.isTrain)
            {
                if(SaveAndLoadGameData.instance.savedData.playerTrains.Any(s => s == p.trainType))
                {
                    buy.interactable = false;
                }
            }
            else if(p.isEnv)
            {
                if(SaveAndLoadGameData.instance.savedData.playerEnvs.Any(s => s == p.envType))
                {
                    buy.interactable = false;
                }
            }
            else if(p.isCarpet)
            {
                if(SaveAndLoadGameData.instance.savedData.playerPlaygrounds.Any(s => s == p.carpetType))
                {
                    buy.interactable = false;
                }
            }
        }
    }
}