using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementManager : MonoBehaviour
{   
    GameUIManager uIManager;
    RailManager railManager;
    ObjectChooser objectChooser;
    PlaygroundManager playgroundManager;
    CameraManager cameraManager;
    RailMover railMover;
    
    [Header("Data")]
    
    public bool isPlacing = false;
    public LayerMask placementLayer;
    PlacementType placementType;  
    GameObject placingObject;
    float height;
    private Touch touch;

    private void Start() 
    {
        cameraManager = FindObjectOfType<CameraManager>();
        uIManager = FindObjectOfType<GameUIManager>();
        railMover = FindObjectOfType<RailMover>();
        playgroundManager = FindObjectOfType<PlaygroundManager>();
        objectChooser = FindObjectOfType<ObjectChooser>();
        railManager = FindObjectOfType<RailManager>();
    }
    void Update()
    {
        if(isPlacing && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
        {
            if(Input.GetMouseButtonDown(0))
            {
                ObjectPlaced();
            }
        }
    }
    void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            PC_FixedUpdate();
        else if (Application.platform == RuntimePlatform.Android)
            Android_FixedUpdate();
    }
    void PC_FixedUpdate()
    {
        if(isPlacing)
        {
            Ray ray = cameraManager.activeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, objectChooser.maxDistance, placementLayer, QueryTriggerInteraction.Collide))
            {
                placingObject.transform.position = new Vector3(hit.point.x, height, hit.point.z);
                if(placementType == PlacementType.RailSystem)
                {
                    placingObject.transform.position = playgroundManager.ClampField(placingObject.transform.position
                        , railMover.minX, railMover.maxX, railMover.minZ
                        ,railMover.maxZ );
                }
                else
                    placingObject.transform.position = playgroundManager.ClampPoisiton(placingObject.transform.position);
            }
        }
    }
    void Android_FixedUpdate()
    {
        if( isPlacing && Input.touchCount > 0 )
        {
            touch = Input.GetTouch(0);

            Ray ray = cameraManager.activeCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, objectChooser.maxDistance, placementLayer, QueryTriggerInteraction.Collide))
            {
                switch (touch.phase)
                {   
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        placingObject.transform.position = new Vector3(hit.point.x, height, hit.point.z);
                        if(placementType == PlacementType.RailSystem)
                        {
                            placingObject.transform.position = playgroundManager.ClampField(placingObject.transform.position
                                , railMover.minX, railMover.maxX, railMover.minZ
                                ,railMover.maxZ );
                        }
                        else
                            placingObject.transform.position = playgroundManager.ClampPoisiton(placingObject.transform.position);
                        break;

                    //Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        placingObject.transform.position = new Vector3(hit.point.x, height, hit.point.z);
                        if(placementType == PlacementType.RailSystem)
                        {
                            placingObject.transform.position = playgroundManager.ClampField(placingObject.transform.position
                                , railMover.minX, railMover.maxX, railMover.minZ
                                ,railMover.maxZ );
                        }
                        else
                            placingObject.transform.position = playgroundManager.ClampPoisiton(placingObject.transform.position);
                        break;

                    case TouchPhase.Ended:
                        ObjectPlaced();
                        break;
                }
                
            }
        }
    }
    void ObjectPlaced()
    {
        isPlacing = false;
        
        AudioManager.instance.Play("ObjectPlacing");

        if(placementType == PlacementType.Rail)
        {
            //placingObject.GetComponent<Rail>().Search();
            placingObject.GetComponent<CollidableBase>().ActivateColliders();
            objectChooser.Choose(placingObject);
        }
        else if(placementType == PlacementType.Env)
        {
            placingObject.GetComponent<CollidableBase>().ActivateColliders();
            objectChooser.Choose(placingObject);
        }
        else if(placementType == PlacementType.RailSystem)
        {
            placingObject.GetComponent<RailMover>().MovingComplated();
        }
        uIManager.buttonLock = false;
        placingObject = null;
    }
    /// <summary>
    /// Call this for place an object.
    /// </summary>
    public void PlaceMe(GameObject obj, PlacementType type)
    {
        if( type != PlacementType.RailSystem && obj.GetComponent<CollidableBase>().isStatic)
            return;    
        placingObject = obj;
        placementType = type;
        if(type == PlacementType.Rail )
        {
            height = railManager.railHeight;
        }
        else if(type == PlacementType.RailSystem)
        {
            height = objectChooser.choosenObject.transform.position.y;
            placingObject.GetComponent<RailMover>().StartMoving();
        }
        isPlacing = true;
    }
    public void PlaceMe(GameObject obj, PlacementType type, float _height)
    {
        placingObject = obj;
        placementType = type;
        height = _height;
        isPlacing = true;
    }
}
public enum PlacementType
{
    Rail, Env, RailSystem
}