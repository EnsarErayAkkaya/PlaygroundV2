﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleBase : CollidableBase, IInteractible
{
    [SerializeField] protected MeshRenderer mesh;
    public bool isSelected;
    public int cost;
    public virtual void Destroy()
    {
        Debug.LogError("Destroy Not implemented");
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public virtual void Glow(bool b)
    {
       //Debug.Log(gameObject.name + " glowing");
    }
    public void HideObject()
    {
        mesh.gameObject.SetActive(false);
    }
    public void ShowObject()
    {
        mesh.gameObject.SetActive(true);
    }
}
