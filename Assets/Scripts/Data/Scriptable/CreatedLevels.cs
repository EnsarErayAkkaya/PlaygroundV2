using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "New CreatedLevels", menuName = "Playground/CreatedLevels", order = 0)]
public class CreatedLevels : ScriptableObject 
{
    public List<LevelData> createdLevels = new List<LevelData>();    
}
