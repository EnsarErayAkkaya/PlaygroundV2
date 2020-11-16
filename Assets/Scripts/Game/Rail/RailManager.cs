using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class RailManager : MonoBehaviour
{
    GameUIManager uIManager;
    ObjectChooser objectChooser;

    [HideInInspector]
    public LightManager lightManager;

    PlaygroundManager playgroundManager;
    ObjectPlacementManager placementManager;
    CameraManager cameraManager;

    // connectingRail yeni ve var olan ray bağlantısı yaparken bağlanan rayı işaret eder
    Rail connectingRail, newCreatedRail;

    // var olan ray bağlantısı yaparken bağlanılan noktayı işaret eder
    RailConnectionPoint connectingPoint,connectionChangingPoint, lastEditedRail;
    Vector3 lastEditedRailPos,lastEditedRailAngle;

    // objectPlacementManagerda kullanılan rayların yüksekliğini deopalayan değişken 
    public float railHeight;

    // nokta seçiliyor mu 
    bool startChoosePointForConnection, startChoosePointForExistingConnection, willStartChoosePointForExistingConnection, mouseReleased;
    public int floorLimit;
    public float firstFloorY, secondFloorY, thirdFloorY, heightLimit;
    [SerializeField] float rotateAngle = 90;
    [SerializeField] List<Rail> rails;
    public uint nextIndex = 0;

     private void Start() {
        cameraManager = FindObjectOfType<CameraManager>();
        uIManager = FindObjectOfType<GameUIManager>();
        playgroundManager = FindObjectOfType<PlaygroundManager>();
        lightManager = FindObjectOfType<LightManager>();
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        objectChooser = FindObjectOfType<ObjectChooser>();
    }

    void Update()
    {  
        if(Input.GetMouseButtonUp(0))
        {
            mouseReleased = true;
        }
    }
    void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            PC_FixedUpdate();
        else if (Application.platform == RuntimePlatform.Android)
            Android_FixedUpdate();
    }

    public RailConnectionPoint ChooseConnectionPoint(RailConnectionPoint startPoint = null)
    {
        RailConnectionPoint railConnectionPoint = null;

        Ray ray = cameraManager.activeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, objectChooser.maxDistance, objectChooser.choosenLayers, QueryTriggerInteraction.Collide))
        {
            if(startPoint != null)
            {
                if(startPoint.isInput == false) // çıkışsa
                {
                    foreach (Rail rail in rails.Where(s => s != startPoint.rail))
                    {
                        foreach (RailConnectionPoint rcp in rail.GetFreeInputConnectionPoints())
                        {
                            if( railConnectionPoint == null || Vector3.Distance(hit.point, railConnectionPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                            {
                                railConnectionPoint = rcp;
                            }
                        } 
                    }
                }
                else
                {
                    foreach (Rail rail in rails.Where(s => s != startPoint.rail))
                    {
                        foreach (RailConnectionPoint rcp in rail.GetFreeOutputConnectionPoints())
                        {
                            if( railConnectionPoint == null || Vector3.Distance(hit.point, railConnectionPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                            {
                                railConnectionPoint = rcp;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Rail rail in rails)
                {
                    foreach (RailConnectionPoint rcp in rail.GetFreeConnectionPoints())
                    {
                        if( railConnectionPoint == null || Vector3.Distance(hit.point, railConnectionPoint.point) 
                                > Vector3.Distance(hit.point, rcp.point) )
                        {
                            railConnectionPoint = rcp;
                        }
                    }
                }
            }
        }
        return railConnectionPoint;
    }
    

    void PC_FixedUpdate()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;

        if(startChoosePointForConnection == true && Input.GetMouseButtonDown(0) && mouseReleased == true )
        {
            Ray ray = cameraManager.activeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, objectChooser.maxDistance, objectChooser.choosenLayers, QueryTriggerInteraction.Collide))
            {
                mouseReleased = false;
                //highlight ı bitir
                connectingRail.DownlightConnectionPoints();

                if(willStartChoosePointForExistingConnection)
                {
                    foreach (RailConnectionPoint rcp in connectingRail.GetConnectionPoints())
                    {
                        if( connectionChangingPoint == null || Vector3.Distance(hit.point, connectionChangingPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                        {
                            connectionChangingPoint = rcp;
                        }
                    }
                    startChoosePointForConnection = false;
                    HighlightRailsForExistingConnection();
                }
                else
                {                    
                    lightManager.OpenLights();
                    foreach (RailConnectionPoint rcp in connectingRail.GetFreeConnectionPoints())
                    {
                        if( connectingPoint == null || Vector3.Distance(hit.point, connectingPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                        {
                            connectingPoint = rcp;
                        }
                    }
                    startChoosePointForConnection = false;
                    
                    Connect();
                }
            }
        }
        if(startChoosePointForExistingConnection == true && Input.GetMouseButtonDown(0) && mouseReleased== true)
        {
            Ray ray = cameraManager.activeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, objectChooser.maxDistance, objectChooser.choosenLayers, QueryTriggerInteraction.Collide))
            {
                DownlightRails();
                lightManager.OpenLights();

                if(connectionChangingPoint.isInput == false) // çıkışsa
                {
                    foreach (Rail rail in rails.Where(s => s != connectingRail))
                    {
                        foreach (RailConnectionPoint rcp in rail.GetFreeInputConnectionPoints())
                        {
                            if( connectingPoint == null || Vector3.Distance(hit.point, connectingPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                            {
                                connectingPoint = rcp;
                            }
                        } 
                    }
                }
                else
                {
                    foreach (Rail rail in rails.Where(s => s != connectingRail))
                    {
                        foreach (RailConnectionPoint rcp in rail.GetFreeOutputConnectionPoints())
                        {
                            if( connectingPoint == null || Vector3.Distance(hit.point, connectingPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                            {
                                connectingPoint = rcp;
                            }
                        } 
                    }
                }
                
                lastEditedRail = connectionChangingPoint;
                lastEditedRailPos = connectionChangingPoint.point;
                lastEditedRailAngle = connectionChangingPoint.transform.rotation.eulerAngles;
                connectionChangingPoint.rail.lastEditTime = Time.time;

                startChoosePointForExistingConnection = false;

                Connect();
            }
        }
    }
    void Android_FixedUpdate()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;

        if(startChoosePointForConnection == true && Input.touchCount > 0 && mouseReleased == true )
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = cameraManager.activeCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, objectChooser.maxDistance, objectChooser.choosenLayers, QueryTriggerInteraction.Collide))
            {
                mouseReleased = false;
                //highlight ı bitir
                connectingRail.DownlightConnectionPoints();

                if(willStartChoosePointForExistingConnection)
                {
                    foreach (RailConnectionPoint rcp in connectingRail.GetConnectionPoints())
                    {
                        if( connectionChangingPoint == null || Vector3.Distance(hit.point, connectionChangingPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                        {
                            connectionChangingPoint = rcp;
                        }
                    }
                    startChoosePointForConnection = false;
                    HighlightRailsForExistingConnection();
                }
                else
                {                    
                    lightManager.OpenLights();
                    foreach (RailConnectionPoint rcp in connectingRail.GetFreeConnectionPoints())
                    {
                        if( connectingPoint == null || Vector3.Distance(hit.point, connectingPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                        {
                            connectingPoint = rcp;
                        }
                    }
                    startChoosePointForConnection = false;
                    
                    Connect();
                }
            }
        }
        if(startChoosePointForExistingConnection == true && Input.touchCount > 0 && mouseReleased== true)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = cameraManager.activeCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, objectChooser.maxDistance, objectChooser.choosenLayers, QueryTriggerInteraction.Collide))
            {
                DownlightRails();
                lightManager.OpenLights();

                if(connectionChangingPoint.isInput == false) // çıkışsa
                {
                    foreach (Rail rail in rails.Where(s => s != connectingRail))
                    {
                        foreach (RailConnectionPoint rcp in rail.GetFreeInputConnectionPoints())
                        {
                            if( connectingPoint == null || Vector3.Distance(hit.point, connectingPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                            {
                                connectingPoint = rcp;
                            }
                        } 
                    }
                }
                else
                {
                    foreach (Rail rail in rails.Where(s => s != connectingRail))
                    {
                        foreach (RailConnectionPoint rcp in rail.GetFreeOutputConnectionPoints())
                        {
                            if( connectingPoint == null || Vector3.Distance(hit.point, connectingPoint.point) 
                                    > Vector3.Distance(hit.point, rcp.point) )
                            {
                                connectingPoint = rcp;
                            }
                        } 
                    }
                }
                
                lastEditedRail = connectionChangingPoint;
                lastEditedRailPos = connectionChangingPoint.point;
                lastEditedRailAngle = connectionChangingPoint.transform.rotation.eulerAngles;
                connectionChangingPoint.rail.lastEditTime = Time.time;

                startChoosePointForExistingConnection = false;

                Connect();
            }
        }
    }
    
    public void AddCreatedRails()
    {
        foreach (Rail item in FindObjectsOfType<Rail>().ToList())
        {
            rails.Add(item);
        }
    }
    void Connect()
    {        
        if(connectingPoint.isInput == false) // bağlanılan nokta çıkış ise
        {
            // direk girişi seçili noktaya bağla

            if(connectionChangingPoint == null)
            {
                connectingPoint.SetConnection( newCreatedRail.GetInputConnectionPoints().FirstOrDefault() );
                AddRail(newCreatedRail);
            }
            else{
                connectingPoint.SetConnection(connectionChangingPoint);
            }

            RailConnection(connectingPoint);

            // açıyı ayarla 
            connectingPoint.connectedPoint.transform.rotation = Quaternion.Euler(connectingPoint.rail.transform.rotation.eulerAngles + new Vector3(0, connectingPoint.extraAngle,0));
        }
        else // bağlanılan nokta giriş ise
        {
            // çıkışı girişe bağla
            
            if(connectionChangingPoint == null)
            {
                connectingPoint.SetConnection(newCreatedRail.GetOutputConnectionPoints().FirstOrDefault());
                AddRail(newCreatedRail);
            }
            else
            {
                connectingPoint.SetConnection(connectionChangingPoint);
            }

            RailConnection(connectingPoint);

            // açıyı ayarla
            connectingPoint.connectedPoint.transform.rotation = Quaternion.Euler(connectingPoint.rail.transform.rotation.eulerAngles - new Vector3(0, connectingPoint.connectedPoint.extraAngle,0));
        }

        ConnectCollidingPoints();
        
        // parentları düzenle
        connectingPoint.connectedPoint.rail.transform.parent = null; // railın parentını tamizle
        connectingPoint.connectedPoint.transform.parent = connectingPoint.connectedPoint.rail.transform; // noktayı railın çocuğu yap

        if( playgroundManager.playground.CheckInPlayground(connectingPoint.connectedPoint.transform) == false )// oyun alanında değilse
        {
            connectingPoint.connectedPoint.rail.Destroy();
        }
        else if(connectingPoint.connectedPoint.rail.FloorControl())// oyun alnındaysa kata bak, kat kontolünü geçtiyse davam et
        {
            if(newCreatedRail != null)
            {
                newCreatedRail.ShowObject();
                newCreatedRail.ActivateColliders();
            }
            objectChooser.Choose(connectingPoint.connectedPoint.rail.gameObject);
        }        
        
        objectChooser.CanChoose();

        AudioManager.instance.Play("ObjectPlacing");
        
        uIManager.buttonLock = false;

        connectingPoint = null;
        newCreatedRail = null;
        connectingRail = null;
        connectionChangingPoint = null;
    }
    void RailConnection(RailConnectionPoint a)
    {
        a.connectedPoint.SetConnection(a);
        
        // connectingPoint noktası bağlanılan nokta ve connectingPoint.connectedPoint bağlanan noktadır
        a.connectedPoint.transform.parent = null; // parentı çıkar
        a.connectedPoint.rail.transform.parent = a.connectedPoint.transform; // raili noktasının çocuğu yap
        a.connectedPoint.point = a.point; // noktaların pozisyonunu birleştir
    }

    void ConnectTwoPoints(RailConnectionPoint standingPoint, RailConnectionPoint changingPoint)
    {
        standingPoint.SetConnection(changingPoint);

        RailConnection(standingPoint);
        // parentları düzenle
        standingPoint.connectedPoint.rail.transform.parent = null; // railın parentını temizle
        standingPoint.connectedPoint.transform.parent = standingPoint.connectedPoint.rail.transform; // noktayı railın çocuğu yap
    }
    // Yeni bir ray oluşturuluyor r bağlanılan ray, nextRail bağlanıcak ray(oluşturulacak)
    public void NewRailConnection(Rail r, GameObject nextRail, int _cost)
    {
        // Ray değilse dön 
        if(nextRail.GetComponent<Rail>() == null ) 
        { 
            Debug.LogError("Attached wrong object to Rail button"); 
            return;
        }        
        connectingRail = r;

        if(connectingRail.GetFreeConnectionPoints().Length > 1)
        {
            objectChooser.CantChoose();

            connectingRail.HighlightConnectionPoints();

            lightManager.CloseLights();

            startChoosePointForConnection = true;

            newCreatedRail = Instantiate( nextRail ).GetComponent<Rail>();
            newCreatedRail.cost = _cost;
            
            newCreatedRail.HideObject();
            newCreatedRail.DisableColliders();

            newCreatedRail.creationTime = Time.time;   
        }
        else if(connectingRail.GetFreeConnectionPoints().Length == 1){
            connectingPoint = r.GetFreeConnectionPoints()[0];
            newCreatedRail = Instantiate( nextRail ).GetComponent<Rail>();
            newCreatedRail.cost = _cost;

            newCreatedRail.HideObject();
            newCreatedRail.DisableColliders();

            newCreatedRail.creationTime = Time.time;           
            Connect();
        }
        else{
            Debug.Log("Bağlanılacak bir nokta yok");
        }
    }
    // it controls free points on connectingRail
    // if a point too close (< 0.1f) it connect points
    // and contuniues with next free point
    void ConnectCollidingPoints()
    {
        if(connectionChangingPoint != null)
        {
            ConnectCollidingRailPoints(connectionChangingPoint.rail);
        }
        else
        {
            ConnectCollidingRailPoints(newCreatedRail);
        }   
    }
    public void ConnectCollidingRailPoints(Rail r)
    {
        foreach (RailConnectionPoint firstPoint in r.GetFreeConnectionPoints())
        {
            bool found = false;

            foreach(Rail rail in rails.Where( s => s != r))
            {
                foreach (var secondPoint in rail.GetFreeConnectionPoints())
                {
                    if(Vector3.Distance(firstPoint.point,secondPoint.point) < 0.1f )
                    {
                        firstPoint.SetConnection(secondPoint);
                        secondPoint.SetConnection(firstPoint);
                        found = true;
                        break;
                    }
                }
                if(found)
                    break;
            }
        }
        r.ManualFloorControl();
    }


    public void ExistingRailConnection(Rail firstRail)
    {
        connectingRail = firstRail;
        if(connectingRail.GetConnectionPoints().Length > 1)
        {
            objectChooser.CantChoose();

            connectingRail.HighlightConnectionPoints( connectingRail.GetConnectionPoints() );
            
            connectingRail.CleanConnections();

            lightManager.CloseLights();

            startChoosePointForConnection = true;
            willStartChoosePointForExistingConnection = true;
        }
        else if(connectingRail.GetConnectionPoints().Length == 1)
        {
            connectionChangingPoint = connectingRail.GetConnectionPoints()[0];
            lightManager.CloseLights();
            HighlightRailsForExistingConnection();
        }
        else
        {
            Debug.Log("Seçebileceğin bir nokta yok");
        }
    }
    public void HighlightRailsForExistingConnection()
    {
        int i = 0;
        if(connectionChangingPoint.isInput == false) // çıkışsa
        {
            foreach (Rail rail in rails.Where(s => s != connectingRail))
            {
                i += rail.HighlightConnectionPoints(rail.GetFreeConnectionPoints().Where(s => s.isInput).ToArray());
            }
        }
        else
        {
            foreach (Rail rail in rails.Where(s => s != connectingRail)) // girişse
            {
                i += rail.HighlightConnectionPoints(rail.GetFreeConnectionPoints().Where(s => !s.isInput).ToArray());
            }
        }
        if(i > 0)
        {
            objectChooser.CantChoose();
            startChoosePointForExistingConnection = true;
        }
        else
        {
            lightManager.OpenLights();
            DownlightRails();
            objectChooser.CanChoose();
            Debug.Log("Uygun bağlanılacak bir nokta yok");
        }
        willStartChoosePointForExistingConnection = false;
    }
    public void HighlightFreeRails(RailConnectionPoint startPoint = null)
    {    
        DownlightRails();

        int i = 0;
        if(startPoint == null) // no rail choosed
        {
            foreach (Rail rail in rails) 
            {
                i += rail.HighlightConnectionPoints(rail.GetFreeConnectionPoints());
            }
        }
        else // already choosed a rail
        {
            if(startPoint.isInput) // girişse
            {
                foreach ( Rail rail in rails.Where(s => s != startPoint.rail) ) 
                {
                    i += rail.HighlightConnectionPoints(rail.GetFreeOutputConnectionPoints());
                }
            }
            else // çıkışsa
            {
                foreach ( Rail rail in rails.Where(s => s != startPoint.rail) ) 
                {
                    i += rail.HighlightConnectionPoints(rail.GetFreeInputConnectionPoints());
                }
            }
        }
        
        if(i > 0)
        {
            objectChooser.CantChoose();
        }
        else
        {
            lightManager.OpenLights();
            DownlightRails();
            objectChooser.CanChoose();
            Debug.Log("Uygun bağlanılacak bir nokta yok");
        }
    }

    public void CreateFloatingRail(GameObject r, int _cost)
    {
        // Ray değilse dön
        if(r.GetComponent<Rail>().floorAdder == -1) 
        { 
            Debug.LogWarning("you cant create this rail to ground");
            uIManager.buttonLock = false; 
            return;
        }
        Rail rail = Instantiate(r).GetComponent<Rail>();
        rail.DisableColliders();
        rail.creationTime = Time.time;
        rail.cost = _cost;
        AddRail(rail);
        placementManager.PlaceMe(rail.gameObject, PlacementType.Rail);
    }
    public void GetRailBackToOldPosition()
    {
        MoveRailToPosition(lastEditedRailPos, lastEditedRailAngle, lastEditedRail);
        ConnectCollidingRailPoints(lastEditedRail.rail);
        objectChooser.Choose(lastEditedRail.rail.gameObject);
    }
    void MoveRailToPosition(Vector3 pos,Vector3 rotation, RailConnectionPoint rcp)
    {
        rcp.rail.CleanConnections();
        // connectingPoint noktası bağlanılan nokta ve connectingPoint.connectedPoint bağlanan noktadır
        rcp.transform.parent = null; // parentı çıkar
        rcp.rail.transform.parent = rcp.transform; // raili noktasının çocuğu yap
        
        rcp.point = pos; // eski pozisyona getir

        rcp.transform.rotation = Quaternion.Euler(rotation); // eski açıya döndür

        rcp.rail.transform.parent = null; // railın parentını temizle
        rcp.transform.parent = rcp.rail.transform; // noktayı railın çocuğu yap

    }
    public void DownlightRails()
    {
        foreach (Rail rail in rails.Where(s => s != connectingRail))
        {
            rail.DownlightConnectionPoints();
        }
    }
    public void FlipRail( Rail r )
    {
        r.transform.localScale = new Vector3( r.transform.localScale.x, r.transform.localScale.y, r.transform.localScale.z * -1 );

        RailConnectionPoint lastConnectedPoint = null, railsConnectedPoint = null;

        bool gotConnectionPointPoints = false;

        foreach (var item in r.GetConnectionPoints())
        {
            if(item && item.hasConnectedRail && !gotConnectionPointPoints )
            {
                lastConnectedPoint = item.connectedPoint;
                railsConnectedPoint = item;
                gotConnectionPointPoints = true;
            }
            item.extraAngle *= -1;
        }

        r.CleanConnections();

        if(lastConnectedPoint != null)
        {
            Debug.Log("points connecting");
            ConnectTwoPoints(lastConnectedPoint,railsConnectedPoint);
        }

        ConnectCollidingRailPoints(r);
    }
    public void RotateRail(Rail r)
    {
        if(!r.isStatic && r.GetFreeConnectionPoints().Length == r.GetConnectionPoints().Length)
        {
            r.transform.RotateAround(r.transform.position, r.transform.up, rotateAngle);
        }
    }
    public List<InteractibleBase> GetConnectedRails(Rail rail)
    {
        List<InteractibleBase> connectedRails = new List<InteractibleBase>();
        foreach (RailConnectionPoint point in rail.GetConnectionPoints())
        {
            if(point.connectedPoint != null && point.connectedPoint.rail.isSelected == false)
            {
                point.connectedPoint.rail.isSelected = true;
                connectedRails.Add( point.connectedPoint.rail );
                connectedRails.AddRange( GetConnectedRails( point.connectedPoint.rail ));
            }
        }
        return connectedRails;
    }
    public void RemoveRail(Rail r)
    {
        rails.Remove(r);
    }
    public void AddRail(Rail r)
    {
        rails.Add(r);

        nextIndex++;
        r.index = nextIndex;
    }
    public void AddRail(Rail r, uint id)
    {
        rails.Add(r);
        r.index = id;
    }
    public Rail GetLastEditedRail()
    {
        if(lastEditedRail != null)
            return lastEditedRail.rail;
        else
            return null;
    }
    public List<Rail> GetRails()
    {
        return rails;
    }
    private void OnDrawGizmos() {
        Gizmos.DrawCube( new Vector3(0,firstFloorY,0), Vector3.one);
        Gizmos.DrawCube( new Vector3(0,secondFloorY,0), Vector3.one);
        Gizmos.DrawCube( new Vector3(0,thirdFloorY,0), Vector3.one);
        Gizmos.DrawCube( new Vector3(0,heightLimit,0), Vector3.one);
    }
}
