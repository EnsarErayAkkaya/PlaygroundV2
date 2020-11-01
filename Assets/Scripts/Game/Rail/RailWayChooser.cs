using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RailWayChooser : MonoBehaviour
{
    [SerializeField] ObjectChooser objectChooser;
    //[SerializeField] LightManager lightManager;
    [SerializeField] TrainManager trainManager;
   
    public void ChangeRailway(Rail rail)
    {
        rail.splineManager.SetNextSpline();
    }
}
