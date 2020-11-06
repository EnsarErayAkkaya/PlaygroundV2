using System.Collections.Generic;
[System.Serializable]
public class LevelData
{
    public int levelIndex;
    public int levelSceneIndex;
    public bool isUnlocked;
    public int mark;
    public int budget;
    public List<RailType> choosenRails;
    public ZenSceneData levelData;
}
