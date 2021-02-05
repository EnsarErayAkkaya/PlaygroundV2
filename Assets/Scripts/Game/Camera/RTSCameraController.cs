using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    public HouseManager houseManager;
    ObjectPlacementManager placementManager;
    public Transform cameraTransform;
    public Camera Camera;
    Touch touch;
    protected Plane Plane;

    #region Movement
        public float movementSpeed = 5f;
        public float rotationSpeed = 3f;
        public float movementTime = 5;
    #endregion

    #region Height
        public float minZoom, maxZoom;
        public Vector3 newZoom;
        public Vector3 zoomAmount; //value in range (0, 1) used as t in Matf.Lerp
        public float minOrtoSize, maxOrtoSize;
        public float ortographicZoom;
    #endregion

    #region Input
        public bool useScreenEdgeInput = true;
        public float screenEdgeBorder = 25f;
        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";
        public string zoomingAxis = "Mouse ScrollWheel";
        private float ScrollWheel
        {
            get { return Input.GetAxis(zoomingAxis); }
        }
        private Vector2 KeyboardInput
        {
            get { return  new Vector3(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));}
        }
        private Vector3 MouseInput
        {
            get { return Input.mousePosition; }
        }
    #endregion
    private Vector3 lastPos;
    private Vector3 newPos;
    
    private void Start() 
    {
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        houseManager = FindObjectOfType<HouseManager>();
        newZoom = cameraTransform.localPosition;
        if(Camera.orthographic)
        {
            ortographicZoom = Camera.orthographicSize;
        }
    }
    private void Update() 
    {
        MovementInput();
        RotationInput();
        ZoomInput();
    }
    public void MovementInput()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if(KeyboardInput.normalized.magnitude != 0)
            {
                Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;   

                Move(desiredMove);
            }
            else if(useScreenEdgeInput)
            {
                Vector3 desiredMove = new Vector3();

                Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
                Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
                Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
                Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

                desiredMove.x = leftRect.Contains(MouseInput) ? -1 : rightRect.Contains(MouseInput) ? 1 : 0;
                desiredMove.z = upRect.Contains(MouseInput) ? 1 : downRect.Contains(MouseInput) ? -1 : 0;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;

                Move(desiredMove);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if( !placementManager.isPlacing && !TouchUtility.IsPointerOverUIObject() )
            {
                if(Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);
                    Plane.SetNormalAndPosition(transform.up, transform.position);

                    var Delta1 = Vector3.zero;
                    var Delta2 = Vector3.zero;

                    Delta1 = PlanePositionDelta(touch);
                    if (touch.phase == TouchPhase.Moved)
                        cameraTransform.Translate(Delta1, Space.World);

                    Vector3 pos = cameraTransform.position;

                    pos.x = Mathf.Clamp(pos.x, houseManager.minX, houseManager.maxX);
                    pos.y = Mathf.Clamp(pos.y, houseManager.minY, houseManager.maxY);
                    pos.z = Mathf.Clamp(pos.z, houseManager.minZ, houseManager.maxZ);

                    transform.position = pos;
                }
            }
        }
    }
    
    public void RotationInput()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if(Input.GetMouseButtonDown(1))
            {
                lastPos = MouseInput;
            }
            else if(Input.GetMouseButton(1))
            {
                newPos = MouseInput;

                Rotate( lastPos - newPos);

                lastPos = newPos;
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //Pinch
            if (Input.touchCount >= 2 && !TouchUtility.IsPointerOverUIObject())
            {
                var pos1  = PlanePosition(Input.GetTouch(0).position);
                var pos2  = PlanePosition(Input.GetTouch(1).position);
                var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
                var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

                //calc zoom
                var zoom = Vector3.Distance(pos1, pos2) /
                        Vector3.Distance(pos1b, pos2b);

                //edge case
                if (zoom == 0 || zoom > 10)
                    return;

                if(!Camera.orthographic)
                {
                    //Move cam amount the mid ray
                    Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);
                }
                
                if (pos2b != pos2)
                    Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
            }
        }
    }
    public void ZoomInput()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if(!Camera.orthographic)
            {
                if(Input.mouseScrollDelta.y != 0)
                    Zoom(Input.mouseScrollDelta.y);
            }
            else
            {
                if(ScrollWheel != 0)
                {
                    ortographicZoom += -ScrollWheel * Time.deltaTime * movementTime * 100;
                    ortographicZoom = Mathf.Clamp(ortographicZoom, minOrtoSize, maxOrtoSize);
                    Camera.orthographicSize = ortographicZoom;
                }
            }
        }
    }

    void Move(Vector3 dir)
    {
        dir *= Time.deltaTime * movementTime;
        dir *= movementSpeed;

        Vector3 pos = transform.position + new Vector3(dir.x, 0, dir.z);

        pos.x = Mathf.Clamp(pos.x, houseManager.minX, houseManager.maxX);
        pos.y = Mathf.Clamp(pos.y, houseManager.minY, houseManager.maxY);
        pos.z = Mathf.Clamp(pos.z, houseManager.minZ, houseManager.maxZ);

        transform.position =pos;
    }
    void Rotate(Vector3 dir)
    {
        dir *= Time.deltaTime * movementTime;
        dir *= rotationSpeed;
        transform.Rotate(Vector3.up, dir.x, Space.World);
    }
    void Zoom(float multiplier)
    {
        newZoom -= multiplier * zoomAmount;

        if(newZoom.y < maxZoom && newZoom.y > minZoom)
        {
            Camera.transform.localPosition = Vector3.Lerp(Camera.transform.localPosition, newZoom, Time.deltaTime * movementTime);
        }

        newZoom = Camera.transform.localPosition;
    }
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }
    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}   
