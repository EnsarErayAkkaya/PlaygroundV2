using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadCreator))]
public class RoadEditor : Editor 
{
    RoadCreator creator;
    private void OnSceneGUI() 
    {
        if(creator.autoUpdate && Event.current.type == EventType.Repaint)
        {
            creator.UpdateRoad();
        }    
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
    }
    private void OnEnable() {
        creator = (RoadCreator)target;
    }
}

