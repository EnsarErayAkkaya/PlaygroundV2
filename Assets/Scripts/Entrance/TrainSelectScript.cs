using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSelectScript : MonoBehaviour
{
    public float maxDistance;
    public LayerMask choosenLayers;
    void FixedUpdate()
    {
        if( TouchUtility.IsPointerOverUIObject() ) return;
       
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            PC_FixedUpdate();
        else if (Application.platform == RuntimePlatform.Android)
            Android_FixedUpdate();
    }
    void PC_FixedUpdate()
    {
        // if we are placing an object
        // we can not choose anything
        if(Input.GetMouseButtonDown(0))
        {
            Select(Input.mousePosition);
        }
    }

    void Android_FixedUpdate()
    {
        // if we are placing an object
        // we can not choose anything
        if( Input.touchCount > 0 )
        {
            Touch touch = Input.GetTouch(0);
            Select(touch.position);
        }
    }
    void Select(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, maxDistance, choosenLayers,QueryTriggerInteraction.Collide))
        {
            if(hit.transform.GetComponent<Train>() != null)
            {
                AudioManager.instance.Play("TrainHorn");
            }
            else if( hit.transform.parent.GetComponent<Train>() != null)
            {
                AudioManager.instance.Play("TrainHorn");
            }
        }
    }
}
