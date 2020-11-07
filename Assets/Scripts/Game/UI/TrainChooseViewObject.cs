using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class TrainChooseViewObject : MonoBehaviour
{
    public TrainType trainType;
    TMP_Dropdown dropdown;
    private void Start() {
        dropdown = transform.GetChild(0).GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(GenericTypeToList.Set<TrainType>());
    }
    public void Remove()
    {
        Destroy(gameObject);
    }
    public void ChooseType()
    {
        trainType = (TrainType)Enum.Parse(typeof(TrainType), dropdown.options[dropdown.value].text);
    }
}
