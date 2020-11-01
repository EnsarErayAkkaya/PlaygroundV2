using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Locomotiv : MonoBehaviour
{
    [System.Serializable]
    public struct LocomotivePosition 
    {
        public Vector3 pose;
        public Quaternion rotation;
    }
    public HingeJoint joint;
    [SerializeField] Train train;
    public List<LocomotivePosition> poses;
    public GameObject vagon;
    [SerializeField]float getNextPosTime, currentSpeed;
    float posTakeTime;
    int posesIndex;
    [SerializeField] int rotationLerpModifier;
    public bool move, started;
    [SerializeField] float normalSpeed, middleSpeed, fastSpeed;
    public Vector3 startingVagonPos;

    void Start()
    {
        posTakeTime = 0;
        startingVagonPos = vagon.transform.position;
    }
    void LateUpdate()
    {
        if(!started )
            return;

        posTakeTime -= Time.deltaTime;

        if(posTakeTime <= 0)
        {
            posTakeTime = getNextPosTime;

            if(posesIndex > poses.Count-1)
            {
                posesIndex = poses.Count-1;
                for (int i = 1; i <= poses.Count-1; i++)
                {
                    poses[i-1] = poses[i];
                }
            }
            poses[posesIndex] = new LocomotivePosition{
                pose = transform.position,
                rotation = transform.rotation
            };
            posesIndex++;
        }
        if(!move )
            return;

        if( poses[0].rotation != null && poses[0].rotation != Quaternion.Euler( Vector3.zero) )
        {
            try
            {
                vagon.transform.position = Vector3.Lerp( vagon.transform.transform.position, poses[0].pose, currentSpeed * Time.deltaTime);
                vagon.transform.rotation = Quaternion.Lerp( vagon.transform.rotation, poses[0].rotation, rotationLerpModifier * Time.deltaTime );
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
    public void ResetVagon()
    {
        vagon.transform.localRotation = Quaternion.Euler( new Vector3(0,-90,0) );
        
        vagon.transform.position = startingVagonPos;

        HingeJoint h = vagon.AddComponent(typeof(HingeJoint)) as HingeJoint;
        
        joint = h;

        if(train.locomotives.Count > 1)
        {
            int j = train.locomotives.FindIndex( s => s == this);
            h.connectedBody = j < 1 ? train.GetComponent<Rigidbody>() : train.locomotives[j-1].GetComponent<Rigidbody>();
        }
        else
        {
            h.connectedBody = train.GetComponent<Rigidbody>();
        }
        
        h.axis = new Vector3(0,1,0);

        for (int i = 0; i < poses.Count; i++)
        {
            poses[i] = new LocomotivePosition{
                pose = Vector3.zero,
                rotation = Quaternion.Euler( Vector3.zero)
            };
        }
    }
    public void SetSpeed()
    {
        if(train.trainManager.speedType == SpeedType.x)
        {
            currentSpeed = normalSpeed;
        }
        else if(train.trainManager.speedType == SpeedType.x2)
        {
            currentSpeed = middleSpeed;
        }
        else if(train.trainManager.speedType == SpeedType.x3)
        {
            currentSpeed = fastSpeed;
        }
    }
    void OnDrawGizmos()
    {
        if(move)
        {
            Vector3 size = new Vector3(.3f,.3f,.3f);
            for (int i = 0; i < poses.Count; i++)
            {
                Gizmos.DrawCube(poses[i].pose, size);
            }
        }
    }
}
