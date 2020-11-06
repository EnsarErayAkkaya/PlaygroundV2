// Base class to store objects in zen scene and reuse them
using UnityEngine;

[System.Serializable]
public class BaseSaveData 
{
    public uint id; 
    public bool isStatic;
    public Vector3Ser position;
    public Vector3Ser rotation;
}
