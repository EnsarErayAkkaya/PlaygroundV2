using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    ObjectPlacementManager placementManager;
    CollectableUIManager collectableUIManager;
    [SerializeField]
    public GameObject prefab;
    public List<TrainCollectable> collectables;

    private void Start() 
    {
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        collectableUIManager = FindObjectOfType<CollectableUIManager>();
    }

    public void CreateCollectable()
    {
        var p = Instantiate(prefab).GetComponent<TrainCollectable>();
        Add(p);
        placementManager.PlaceMe( p.gameObject, PlacementType.Collectable, 0 );
    }
    public void OnReset()
    {
        if(FindObjectOfType<LevelManager>() == null)
            return;
        
        while(collectables.Count > 0)
        {
            if( collectables[0] != null && collectables[0].gameObject != null)
                collectables[0].Destroy();
            collectables.RemoveAt(0);
        }

        foreach (var item in GameDataManager.instance.zenSceneDataManager.LoadingScene.collectableData)
        {
            TrainCollectable c = Instantiate( prefab , item.position, Quaternion.Euler(item.rotation) ).GetComponent<TrainCollectable>();
            Add(c);
            c.isStatic = item.isStatic;            
        }
        
        collectableUIManager.Set(); 
    }
    public void Rotate(GameObject g)
    {
        g.transform.Rotate(0, 90, 0);
    }
    public void Add(TrainCollectable c)
    {
        collectables.Add(c);
    }
}
