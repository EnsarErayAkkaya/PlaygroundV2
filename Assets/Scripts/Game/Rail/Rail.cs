﻿using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
public class Rail : InteractibleBase
{
    [Header("Referances")]
    public SplineManager splineManager;
    RailManager railManager;
    ObjectPlacementManager placementManager;
    ObjectChooser objectChooser;
    RailMover railMover;
    LevelUI levelUI;

    [Header("Data")]
    RailConnectionPoint currentOutputPoint;// active way
    public int floorAdder;
    public int currentFloor;

    public uint index;
    public RailType railType;

    // Bir sonraki rayların bağlanabileceği noktaların serisi
    [SerializeField] RailConnectionPoint[] connectionPoints;

    public Animator animator;

    public bool isStart, isEnd;


    void Start()
    {
        railManager = FindObjectOfType<RailManager>();
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        objectChooser = FindObjectOfType<ObjectChooser>();
        railMover = FindObjectOfType<RailMover>();
        levelUI = FindObjectOfType<LevelUI>();

        currentOutputPoint = GetOutputConnectionPoints().FirstOrDefault();
        if (levelUI != null && isEnd)
        {
            var go = Instantiate(levelUI.finishLine, transform);
            if (transform.localScale.z == -1)
                go.transform.localScale = new Vector3(-1, 1, 1);
            go.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        if (isStatic)
        {
            ManualFloorControl();
        }
    }
    public void OnCollisionCallBack(CollidableBase collidedObject)
    {
        if (isMoving)
        {
            animator.Play("InteractibleCollision");
            railMover.ChildCollidedCallBack();
        }
        else if (!isMoving &&
            (lastCollided == null || (collidedObject.GetHashCode() != lastCollided.GetHashCode()) || Time.time - lastCollisionTime > .9f))
        {
            lastCollided = collidedObject;
            lastCollisionTime = Time.time;
            animator.Play("InteractibleCollision");
            if (!this.isStatic) // çarpıştığım obje statik ve ben değilsem
            {
                if (railManager.GetLastEditedRail() == null || (railManager.GetLastEditedRail().GetHashCode() != this.GetHashCode()
                    && Time.time - collidedObject.lastEditTime > .9f)) // kıpırdamadım diğeri de kıpırdamamış
                {
                    if (this.creationTime > collidedObject.creationTime) // ben yeni mi yerleştim
                    {
                        //siliniyorum
                        Destroy();
                    }
                }
                else if (railManager.GetLastEditedRail() != null && railManager.GetLastEditedRail() == this)
                {
                    if (this.lastEditTime > collidedObject.creationTime) // obje oluştuktan sonra kıpırdamışım
                    {
                        // kıpırdamışım
                        // geri yeri me dönüyorum
                        railManager.GetRailBackToOldPosition();
                    }
                }
            }
        }
    }
    public void SetCurrentOutputPoint(RailConnectionPoint point)
    {
        currentOutputPoint = point;
    }
    public Rail GetNextRail()
    {
        return currentOutputPoint.connectedPoint.rail;
    }
    public RailConnectionPoint GetCurrentConnectionPoint()
    {
        return currentOutputPoint;
    }
    public bool HasNextRail()
    {
        return currentOutputPoint.hasConnectedRail;
    }
    public void SetRailWayOptionAuto(RailConnectionPoint inputPoint)
    {
        splineManager.SetSpline(inputPoint, currentOutputPoint);
    }
    /// <summary>
    /// Delete rail properly
    /// </summary>
    public override void Destroy()
    {
        // If this rail is static you cant delete it 
        if (isStatic)
            return;

        if (levelUI != null)
            levelUI.SetBudget(cost);

        if (railManager == null)
            railManager = FindObjectOfType<RailManager>();

        // Remove from list
        railManager.RemoveRail(this);

        CleanConnections();

        Destroy(gameObject);
    }
    // nulls all connections
    public void CleanConnections()
    {
        //Clean connectionPoints, connectedPoints 
        foreach (RailConnectionPoint item in connectionPoints)
        {
            if (item.connectedPoint != null)
            {
                item.connectedPoint.connectedPoint = null;
                item.connectedPoint.hasConnectedRail = false;

                item.hasConnectedRail = false;
                item.connectedPoint = null;
            }
        }
    }
    // eğer kat uygunsa true değilse false dönder.
    public bool FloorControl()
    {
        RailConnectionPoint rcp = connectionPoints.First(s => s.connectedPoint != null);
        if (rcp.isInput)
            currentFloor = rcp.connectedPoint.rail.currentFloor + floorAdder;
        else
            currentFloor = rcp.connectedPoint.rail.currentFloor - floorAdder;

        if (railManager == null)
            railManager = FindObjectOfType<RailManager>();

        if (currentFloor < 0 || currentFloor > railManager.floorLimit)
        {
            Destroy();
            return false;
        }
        else
            return true;
    }
    public void ManualFloorControl()
    {
        if (railManager == null)
            railManager = FindObjectOfType<RailManager>();

        if (transform.position.y <= railManager.firstFloorY)
        {
            currentFloor = 0;
        }
        else if (transform.position.y >= railManager.firstFloorY && transform.position.y <= railManager.secondFloorY)
        {
            currentFloor = 1;
        }
        else if (transform.position.y >= railManager.secondFloorY && transform.position.y <= railManager.thirdFloorY)
        {
            currentFloor = 2;
        }
        else if (transform.position.y >= railManager.thirdFloorY && transform.position.y <= railManager.heightLimit)
        {
            currentFloor = 3;
        }
        else if (transform.position.y >= railManager.heightLimit || transform.position.y < 0)
        {
            Destroy();
        }
    }
    public override void Glow(bool b)
    {
        if (b)
        {
            mesh.material.SetInt("Vector1_114B864B", 3);
            if (mesh.materials.Length > 1)
                mesh.materials[1].SetInt("Vector1_114B864B", 3);
        }
        else
        {
            mesh.material.SetInt("Vector1_114B864B", 0);
            if (mesh.materials.Length > 1)
                mesh.materials[1].SetInt("Vector1_114B864B", 0);
        }
    }

    //All Connection points
    public RailConnectionPoint[] GetConnectionPoints()
    {
        return connectionPoints;
    }

    // Conection Points with no connection
    public RailConnectionPoint[] GetFreeConnectionPoints()
    {
        return connectionPoints.Where(s => s.connectedPoint == null).ToArray();
    }
    public RailConnectionPoint[] GetFreeOutputConnectionPoints()
    {
        return connectionPoints.Where(s => s.connectedPoint == null && s.isInput == false).ToArray();
    }
    public RailConnectionPoint[] GetFreeInputConnectionPoints()
    {
        return connectionPoints.Where(s => s.connectedPoint == null && s.isInput == true).ToArray();
    }
    public RailConnectionPoint[] GetOutputConnectionPoints()
    {
        return connectionPoints.Where(s => s.isInput == false).ToArray();
    }
    public RailConnectionPoint[] GetInputConnectionPoints()
    {
        return connectionPoints.Where(s => s.isInput == true).ToArray();
    }
    // Highlights all points
    public int HighlightConnectionPoints()
    {
        int i = 0;
        foreach (RailConnectionPoint item in GetFreeConnectionPoints())
        {
            i++;
            item.Highlight();
        }
        return i;
    }
    // highlights given points
    public int HighlightConnectionPoints(RailConnectionPoint[] rs)
    {
        int i = 0;
        foreach (RailConnectionPoint item in rs)
        {
            i++;
            item.Highlight();
        }
        return i;
    }
    //Downlight points
    public void DownlightConnectionPoints()
    {
        // Downlight highlighted points
        foreach (RailConnectionPoint item in GetConnectionPoints().Where(h => h.isHighlighted))
        {
            item.Downlight();
        }
    }
}