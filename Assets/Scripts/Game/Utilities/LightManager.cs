using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] Light light;
    private void Start() {
        if(light == null)
        {
            light = transform.GetChild(0).GetComponent<Light>();
        }
        
    }

    public void CloseLights()
    {
        light.intensity = 0;
    }
    public void OpenLights()
    {
        light.intensity = 1;
    }
}
