using System.Collections.Generic;
[System.Serializable]
public class LevelData
{
    public string levelName;
    public int levelIndex;
    public int levelSceneIndex;
    public LevelContentData levelContent;
    public ZenSceneData levelData;
    public bool isMaster;
}
