using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RailChoosePanel : MonoBehaviour
{
    public Transform scrollViewContent;
    public GameObject railScrollViewPrefab;
    
    public void AddNewRailToList()
    {
        Instantiate(railScrollViewPrefab, scrollViewContent);
    }
    public List<RailType> GetChoosenRails()
    {
        List<RailType> rails = new List<RailType>();
        foreach (Transform item in scrollViewContent.transform)
        {
            rails.Add(item.GetComponent<RailChooseViewObject>().railType);
        }
        return rails;
    }
    
}
