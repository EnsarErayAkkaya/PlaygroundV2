using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    ObjectPlacementManager placementManager;
    [SerializeField]
    public GameObject prefab;

    private void Start() 
    {
        placementManager = FindObjectOfType<ObjectPlacementManager>();    
    }

    public void CreateCollectable()
    {
        var p = Instantiate(prefab);
        placementManager.PlaceMe( p, PlacementType.Collectable, 0 );
    }
    public void Rotate(GameObject g)
    {
        g.transform.Rotate(0, 90, 0);
    }
}
