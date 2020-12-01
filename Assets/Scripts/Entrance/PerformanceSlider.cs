using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceSlider : MonoBehaviour
{
    [SerializeField] Slider slider;

    void OnEnable()
    {
        slider.value = SaveAndLoadGameData.instance.savedData.interactibleContentValue;
    }
    public void SetInteractibleContentValue()
    {
        GameDataManager.instance.SetInteractibleContentValue((int)slider.value);
    }
}
