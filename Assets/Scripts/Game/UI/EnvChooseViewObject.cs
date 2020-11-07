using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnvChooseViewObject : MonoBehaviour
{
    public EnvType envType;
    TMP_Dropdown dropdown;
    private void Start() {
        dropdown = transform.GetChild(0).GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(GenericTypeToList.Set<EnvType>());
    }
    public void Remove()
    {
        Destroy(gameObject);
    }
    public void ChooseType()
    {
        envType = (EnvType)Enum.Parse(typeof(EnvType), dropdown.options[dropdown.value].text);
    }
}
