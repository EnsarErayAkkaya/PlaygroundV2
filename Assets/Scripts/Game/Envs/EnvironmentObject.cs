using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : InteractibleBase
{
    ObjectPlacementManager placementManager;
    EnvironmentManager environmentManager;
    LevelUI levelUI;

    [SerializeField] float height;
    public EnvType envType;
    
    void Start()
    {
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        environmentManager = FindObjectOfType<EnvironmentManager>();
        levelUI = FindObjectOfType<LevelUI>();
    }
    public void StartPlacing()
    {
        DisableColliders(); 
        if(placementManager == null)
            placementManager = FindObjectOfType<ObjectPlacementManager>();
        if( !isStatic )
            placementManager.PlaceMe(gameObject,PlacementType.Env, height);
    }
    void OnTriggerEnter(Collider other)
    {
        CollidableBase collidedObject = null;
        if(other.GetComponent<CollidableBase>() != null )
            collidedObject = other.GetComponent<CollidableBase>();
        else if(other.transform.parent.GetComponent<CollidableBase>() != null)
            collidedObject = other.transform.parent.GetComponent<CollidableBase>();
        else
            collidedObject = other.transform.parent.parent.GetComponent<CollidableBase>();

        if( lastCollided == null || (collidedObject.GetHashCode() != lastCollided.GetHashCode()) || Time.time - lastCollisionTime > .9f )
        {
            lastCollided =  collidedObject;
            lastCollisionTime = Time.time;
            if(!this.isStatic) // çarpıştığım obje statik ve ben değilsem
            {
                if(this.creationTime > collidedObject.creationTime) // oluşmuşum ve çarpmışım
                {
                    Destroy();
                }
                else  if(this.lastEditTime > collidedObject.creationTime) // kıpırdamışım ve çarpmışım
                {
                    Destroy();
                    // get back to old pos
                } 
            }   
        }       
    }
    
    public override void Destroy()
    {
        // If this rail is static you cant delete it 
        if(isStatic)
            return;

        if(environmentManager == null)
            environmentManager = FindObjectOfType<EnvironmentManager>();
        
        // Remove from list
        environmentManager.RemoveEnv(this);

        if(levelUI != null)
            levelUI.SetBudget( cost );

        Destroy(gameObject);    
    }
}
