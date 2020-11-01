using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
using System.Linq;
public class Train : InteractibleBase
{
    [HideInInspector]public TrainManager trainManager;
    [SerializeField] BezierWalkerWithSpeed walker;
    public List<Locomotiv> locomotives;
    public ParticleSystem particleSystem;
    public Rail rail;
    public bool started;
    public TrainType trainType;
    public uint startingRailId;
    bool reachedTargetRail;
    public Vector3 startingPos;
    void Start()
    {
        particleSystem.Stop();
        trainManager = FindObjectOfType<TrainManager>();
        
        if(FindObjectOfType<EntranceUI>() != null)
            StartTrain();
        else
            SetSpeed();
        
        startingPos = transform.position;
    }
    void OnTriggerEnter(Collider other)
    {
        CollidableBase collidedObject = null;

        if(other.GetComponent<CollidableBase>() != null )
        {
            collidedObject = other.GetComponent<CollidableBase>();
            if(trainManager.isStarted)
            {
                TrainCollision();
            }
            else
            {
                OnCollision(collidedObject);                
            }
        }
        else if(other.transform.parent.GetComponent<CollidableBase>() != null)
        {
            collidedObject = other.transform.parent.GetComponent<CollidableBase>();
            if(trainManager.isStarted)
            {
                TrainCollision();
            }
            else
            {
                OnCollision(collidedObject);                
            }
        }
    }
    void OnCollision(CollidableBase collidedObject)
    {
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
                } 
            } 
        }
    }
    void TrainCollision()
    {
        trainManager.StopAllTrains();
        trainManager.lockTrains = true;
        //some explosion effect maybe
        trainManager.OnTrainsCollided();
    }
    public void Update()
    {
        if(started && walker.spline.splineEnded)
        {   
            BezierSpline exSpline = walker.spline;
            if(rail.HasNextRail() == true )
            {                
                Rail nextRail = rail.GetNextRail();
                if( nextRail.GetConnectionPoints().Length > 2 && nextRail.GetOutputConnectionPoints().Length  == 1 )
                {
                    nextRail.SetRailWayOptionAuto(rail.GetCurrentConnectionPoint().connectedPoint);
                }

                rail = nextRail;
                walker.NormalizedT = 0;
                walker.spline = rail.splineManager.bezierSpline;

                if(!reachedTargetRail)
                    reachedTargetRail = trainManager.OnTrainReachedRail(rail);
            }
            else
            {
                StopTrain();

                if(!reachedTargetRail)
                    reachedTargetRail = trainManager.OnTrainReachedRail(rail);

                walker.spline = null;
                started = false;
            }
            exSpline.SetPathEndedFalse();
        }
        
    }

    public void StopTrain()
    {
        if(started == true)
        {
            walker.move = false;
            foreach (var item in locomotives)
            {
                item.move = false;
                item.started = false;
            }
            
            particleSystem.Stop();
        }
    }
    public void ResumeTrain()
    {
        if(started == true)
        {
            walker.move = true;
            foreach (var item in locomotives)
            {
                item.move = true;
                item.started = true;
            }
            particleSystem.Play();
        }
    }
    public void StartTrain()
    {
        if(started == false)
        {
            if(rail == null)
            {
                rail = FindObjectOfType<RailManager>().GetRails()[0];
                Debug.Log("Selecting first rail, there is no attached rail to " + gameObject.name);
            }

            walker.spline = rail.splineManager.bezierSpline;

            walker.move = true;
            started = true;

            particleSystem.Play();

            StartCoroutine( WaitForLocomotive() );
        }
    }

    IEnumerator WaitForLocomotive()
    {
        foreach (var item in locomotives)
        {
            item.started = true;
        }
        yield return new WaitForSeconds(1f);

        foreach (var item in locomotives)
        {
            Destroy(item.joint);
            item.move = true;
        }
    }
    
    public void SetSpeed()
    {
        if(trainManager.speedType == SpeedType.x)
        {
            walker.speed = trainManager.normalSpeed;
        }
        else if(trainManager.speedType == SpeedType.x2)
        {
            walker.speed = trainManager.middleSpeed;
        }
        else if(trainManager.speedType == SpeedType.x3)
        {
            walker.speed = trainManager.fastSpeed;
        }
        foreach (var item in locomotives)
        {
            item.SetSpeed();
        }
    }
    public void Reset()
    {
        StopTrain();
        started = false;
        
        walker.spline.splineEnded = false;

        walker.NormalizedT = 0;
        
        Rail r = FindObjectsOfType<Rail>().First( s => s.index == startingRailId);
        
        rail = r;
        
        transform.localRotation =  Quaternion.Euler( new Vector3(0,-90,0) );

        transform.position = startingPos;

        foreach (var item in locomotives)
        {
            item.ResetVagon();
        }               
    }
    public override void Destroy()
    {
        trainManager.RemoveTrain(this);

        Destroy(transform.parent.gameObject);
    }
}
[System.Serializable]
public enum SpeedType
{
    x,x2,x3
}
