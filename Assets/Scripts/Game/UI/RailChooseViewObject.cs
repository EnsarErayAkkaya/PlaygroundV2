using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RailChooseViewObject : MonoBehaviour
{
    public RailType railType;
    TMP_Dropdown dropdown;
    private void Start() {
        dropdown = transform.GetChild(0).GetComponent<TMP_Dropdown>();
    }
    public void Remove()
    {
        Destroy(gameObject);
    }
    public void ChooseType()
    {
        railType = (RailType)Enum.Parse(typeof(RailType), dropdown.options[dropdown.value].text);
    }
}
