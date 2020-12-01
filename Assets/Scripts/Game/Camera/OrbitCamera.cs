using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Camera Camera;
    public Transform cameraTransform;
    public Transform target;
    public float speed = 2.0f;
    public Vector3 offSet;
	float currentZoom = 10f;
    public float ZoomSpeed = 4f , MinZoom = 5f , MaxZoom = 15f;
    private Touch touch;
    private Vector3 lastPos;
    private Vector3 newPos;
    protected Plane Plane;
    void LateUpdate()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer)
            PC_Update();
        else if (Application.platform == RuntimePlatform.Android)
            ANDROID_Update();
    }
    void PC_Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom,MinZoom,MaxZoom);

        if(Input.GetMouseButton(0))
        {
            offSet = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speed,Vector3.up) * offSet;
        }
        transform.position = target.position - offSet * currentZoom;
        transform.LookAt(target);
    }
    void ANDROID_Update()
    {
        if (Input.touchCount == 1 && !TouchUtility.IsPointerOverUIObject())
        {
            touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastPos = touch.position;
                break;

                case TouchPhase.Moved:
                    newPos = touch.position;
                    Rotate( (lastPos - newPos).normalized);
                    lastPos = newPos;
                break;
            }
        }
        else if (Input.touchCount > 1 && !TouchUtility.IsPointerOverUIObject())
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            float zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);
            zoom = Mathf.Clamp(currentZoom, -1, 1);

            currentZoom += zoom * ZoomSpeed;

            currentZoom = Mathf.Clamp(currentZoom, MinZoom, MaxZoom);
            
            offSet = Quaternion.AngleAxis(currentZoom * speed, Vector3.up) * offSet;
        }
        transform.position = target.position - offSet * currentZoom;
        transform.LookAt(target);
    }
    void Rotate(Vector3 dir)
    {
        dir *= Time.deltaTime / 2;
        dir *= speed;

        offSet = Quaternion.AngleAxis( -dir.x * speed,Vector3.up ) * offSet;
    }
    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }
}
