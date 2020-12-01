using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelCreationManager : MonoBehaviour
{

    public TMP_InputField budgetField;
    public TMP_InputField nameField;
    public GameObject replaceLevelButton;

    [Header("Level Content" )]
    public Transform levelRailsContent;
    public Transform levelEnvsContent;
    public Transform levelTrainsContent;

    [Header("Reward Content" )]
    public Transform rewardRailsContent;
    public Transform rewardEnvsContent;
    public Transform rewardTrainsContent;
    [Header("Prefabs")]
    
    public GameObject railScrollViewPrefab;
    public GameObject envScrollViewPrefab;
    public GameObject trainsScrollViewPrefab;
    void OnEnable()
    {
        if (GameDataManager.instance.zenSceneDataManager.isLevel)
            replaceLevelButton.SetActive(true);
    }
    
    #region rails
    public void AddNewLevelRailToList()
    {
        Instantiate(railScrollViewPrefab, levelRailsContent);
    }
    public void AddNewRewardRailToList()
    {
        Instantiate(railScrollViewPrefab, rewardRailsContent);
    }
    List<RailType> GetChoosenRails()
    {
        List<RailType> rails = new List<RailType>();
        foreach (Transform item in levelRailsContent.transform)
        {
            rails.Add(item.GetComponent<RailChooseViewObject>().railType);
        }
        return rails;
    }
    List<RailType> GetRewardRails()
    {
        List<RailType> rails = new List<RailType>();
        foreach (Transform item in rewardRailsContent.transform)
        {
            rails.Add(item.GetComponent<RailChooseViewObject>().railType);
        }
        return rails;
    }
    #endregion

    #region envs
    public void AddNewLevelEnvsToList()
    {
        Instantiate(envScrollViewPrefab, levelEnvsContent);
    }
    public void AddNewRewardEnvToList()
    {
        Instantiate(envScrollViewPrefab, rewardEnvsContent);
    }
    List<EnvType> GetChoosenEnvs()
    {
        List<EnvType> envs = new List<EnvType>();
        foreach (Transform item in levelEnvsContent.transform)
        {
            envs.Add(item.GetComponent<EnvChooseViewObject>().envType);
        }
        return envs;
    }
    List<EnvType> GetRewardEnvs()
    {
        List<EnvType> envs = new List<EnvType>();
        foreach (Transform item in rewardEnvsContent.transform)
        {
            envs.Add(item.GetComponent<EnvChooseViewObject>().envType);
        }
        return envs;
    }
    #endregion 

    #region trains
    public void AddNewLevelTrainToList()
    {
        Instantiate(trainsScrollViewPrefab, levelTrainsContent);
    }
    public void AddNewRewardTrainToList()
    {
        Instantiate(trainsScrollViewPrefab, rewardTrainsContent);
    }
    public List<TrainType> GetChoosenTrains()
    {
        List<TrainType> trains = new List<TrainType>();
        foreach (Transform item in levelTrainsContent.transform)
        {
            trains.Add(item.GetComponent<TrainChooseViewObject>().trainType);
        }
        return trains;
    }
    List<TrainType> GetRewardTrains()
    {
        List<TrainType> trains = new List<TrainType>();
        foreach (Transform item in rewardTrainsContent.transform)
        {
            trains.Add(item.GetComponent<TrainChooseViewObject>().trainType);
        }
        return trains;
    }
    #endregion 
 
    public LevelContentData GetLevelContentData()
    {
        return new LevelContentData()
        {
            levelName = nameField.text,
            budget = SetBudget(),
            levelRails = GetChoosenRails(),
            rewardRails = GetRewardRails(),
            
            levelEnvs = GetChoosenEnvs(),
            rewardEnvs = GetRewardEnvs(),
            
            levelTrains = GetChoosenTrains(),
            rewardTrains = GetRewardTrains(),
        };
    }


    public int SetBudget()
    {
        int budget = int.Parse(budgetField.text);
        if(budget > 5000)
            budget = 5000;
        else if(budget < 100 )
            budget = 1000;
        return budget;
    } 
}
