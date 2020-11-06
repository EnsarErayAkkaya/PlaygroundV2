using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartEndToggles : MonoBehaviour
{
    private GameUIManager gameUIManager;

    public Toggle start, end;

    private void Start() {
        gameUIManager = FindObjectOfType<GameUIManager>();
    }

    public void SetToggles( Rail r )
    {
        ShowToggles();
        start.isOn = r.isStart;
        end.isOn = r.isEnd;
    }
    public void ShowToggles()
    {
        start.gameObject.SetActive(true);
        end.gameObject.SetActive(true);
    }
    public void HideToggles()
    {
        start.gameObject.SetActive(false);
        end.gameObject.SetActive(false);
    }
    public void StartToggleValueChanged()
    {
        gameUIManager.GetChoosed().GetComponent<Rail>().isStart = start.isOn;
    }

    public void EndToggleValueChanged()
    {
        gameUIManager.GetChoosed().GetComponent<Rail>().isEnd = end.isOn;
    }
}
