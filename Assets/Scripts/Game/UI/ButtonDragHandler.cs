using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    GameUIManager uIManager;
    int type = 0;
    GameObject obj;
    int cost = 0;
    bool dragLock;

    private void Start()
    {
        uIManager = FindObjectOfType<GameUIManager>();
    }
    /// <summary>
    ///  _type == 0 => rail
    ///  _type == 1 => env
    ///  _type == 2 => train
    /// </summary>
    /// <param name="_type"></param>
    public void SetDrager(int _type, GameObject g ,int _cost)
    {
        type = _type;
        obj = g;
        cost = _cost;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (dragLock) return;

        if(type == 0)
        {
            uIManager.RailButtonClick(obj, cost);
        }
        else if (type == 1)
        {
            uIManager.EnvironmentCreateButtonClick(obj, cost);
        }
        dragLock = true;
        /*else if (type == 2)
        {
            uIManager.(obj, cost);
        }*/
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragLock = false;
    }
}
