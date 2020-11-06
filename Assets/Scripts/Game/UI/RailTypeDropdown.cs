using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;
public class RailTypeDropdown : MonoBehaviour
{
    private void Start() {
        TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(Enum.GetNames(typeof(RailType)).ToList());
    }
}
