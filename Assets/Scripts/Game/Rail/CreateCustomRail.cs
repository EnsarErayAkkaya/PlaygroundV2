using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCustomRail : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField]
    private  RailManager railManager;

    [Header("Data")]
    [SerializeField]
    private GameObject railPartPrefab;
    
    [SerializeField]
    private Transform customRailParent;
    
    
    [SerializeField]
    private float railPartLength = 0.75f;

    [SerializeField]
    private Vector3 distanceBeetwenPoints;
    
    [SerializeField]
    private RailConnectionPoint startPoint;
    [SerializeField]
    
    private RailConnectionPoint endPoint;
    
    [SerializeField]
    private bool chooseStartPoint, chooseEndPoint, mouseReleased;


    public void CreateCustomRailway( RailConnectionPoint StartPoint, RailConnectionPoint EndPoint )
    {
        Debug.Log("creating custom rail");
        startPoint = StartPoint;
        endPoint = EndPoint;

        distanceBeetwenPoints = endPoint.point - startPoint.point;
        Debug.Log("distance beetwen points: "+ distanceBeetwenPoints );

        int railPartCount = Mathf.Abs( (int)( distanceBeetwenPoints.z / railPartLength ) );
        //var parent = Instantiate( customRailParent, (endPoint.point - ( startPoint.point /2 )), Quaternion.identity );
        Debug.Log(railPartCount );
        float zPoint = 0;

        for (int i = 0; i < railPartCount; i++)
        {
            var newRailPart = Instantiate(railPartPrefab, customRailParent);
            newRailPart.transform.position = startPoint.point + new Vector3(0, 0, -zPoint);
            zPoint += railPartLength;
        }
    } 
    private void Update() 
    {
        if(Input.GetMouseButtonUp(0))
        {
            mouseReleased = true;
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            StartCreateCustomRail();
        }
    }
    private void FixedUpdate() 
    {
        if(chooseStartPoint)
        {
            if(Input.GetMouseButtonDown(0) && mouseReleased == true )
            {
                mouseReleased = false;
                startPoint = railManager.ChooseConnectionPoint(null);
                chooseStartPoint = false;
                ChooseEndPoint();
            }
        }
        if(chooseEndPoint)
        {
            if(Input.GetMouseButtonDown(0) && mouseReleased == true )
            {
                mouseReleased = false;
                endPoint = railManager.ChooseConnectionPoint(startPoint);
                chooseEndPoint = false;
                CreateCustomRailway(startPoint, endPoint);
                railManager.lightManager.OpenLights();
                railManager.DownlightRails();
            }
        }
        
    }
    public void StartCreateCustomRail()
    {
        railManager.lightManager.CloseLights();
        railManager.HighlightFreeRails();
        chooseStartPoint = true;
    }
    public void ChooseEndPoint()
    {
        railManager.HighlightFreeRails(startPoint);
        chooseEndPoint = true;
    }
}
