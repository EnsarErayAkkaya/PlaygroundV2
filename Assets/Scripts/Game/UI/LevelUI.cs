using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class LevelUI : MonoBehaviour
{
    [HideInInspector]
    public LevelManager levelManager;
    [SerializeField] GameObject endUI;
    [SerializeField] GameObject adButton;
    [SerializeField] Transform rewardsContent;
    [SerializeField] Image markImage;
    [SerializeField] Button nextButton,restartButton;
    [SerializeField] TextMeshProUGUI lostText, budgetText;

    public Sprite[] stars;

    public GameObject finishLine;
    public GameObject rewardImagePrefab;

    void Start()
    {
        adButton.SetActive(false);
        if(levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();
    }
    public bool SetBudget(int c)
    {
        bool a = levelManager.SetBudget(c);
        if(a)
        {
            SetBudgetText();
            return true;
        }
        else
        {
            Debug.Log("Not enough resources");
            return false;
        }
    }
    public void SetBudgetText()
    {
        budgetText.text = levelManager.budget.ToString();
    }
    public void SetEndUI(int m)
    {
        endUI.SetActive(true);

        if(m == 0 ) // you lost
        {
            markImage.gameObject.SetActive(false);
            lostText.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }
        else // you passed
        {
            if(!GameDataManager.instance.CheckLevelAlreadyPassed())
            {
                adButton.gameObject.SetActive(true);
            }

            ShowRewards();

            markImage.sprite = stars[stars.Length - m];
        
            if( GameDataManager.instance.levels.Any(s => s.levelIndex == GameDataManager.instance.currentlyPlayingLevelIndex + 2) )
                nextButton.gameObject.SetActive(true);
            else
                nextButton.gameObject.SetActive(false);
        }
    }
    void ShowRewards()
    {
        // Clean
        foreach (Transform child in rewardsContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in levelManager.levelRailPrize)
        {
            RailData data = GameDataManager.instance.allRails.Find(s => s.railType == item);

            GameObject e = Instantiate(rewardImagePrefab, rewardsContent);
            e.GetComponent<Image>().sprite = data.railImage;
        }
        foreach (var item in levelManager.levelEnvPrize)
        {
            EnvironmentData data = GameDataManager.instance.allEnvs.Find(s => s.envType == item);

            GameObject e = Instantiate(rewardImagePrefab, rewardsContent);
            e.GetComponent<Image>().sprite = data.envImage;
        }
        foreach (var item in levelManager.levelTrainPrize)
        {
            TrainData data = GameDataManager.instance.allTrains.Find(s => s.trainType == item);

            GameObject e = Instantiate(rewardImagePrefab, rewardsContent);
            e.GetComponent<Image>().sprite = data.trainImage;
        }
    }
    public void NextLevelButtonClick()
    {
        AudioManager.instance.Stop("Celebration");
        AudioManager.instance.Stop("TrainMoving");
        LevelData ld = GameDataManager.instance.levels.First( s => s.levelIndex == GameDataManager.instance.currentlyPlayingLevelIndex + 2 );
        GameDataManager.instance.currentlyPlayingLevelIndex = ld.levelIndex - 1;
        GameDataManager.instance.zenSceneDataManager.LoadingScene = ld.levelData;
        GameDataManager.instance.zenSceneDataManager.isLoad = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RestartLevelButtonClick()
    {
        AudioManager.instance.Stop("Celebration");
        AudioManager.instance.Stop("TrainMoving");
        LevelData ld = GameDataManager.instance.levels.First( s => s.levelIndex == GameDataManager.instance.currentlyPlayingLevelIndex + 1 );
        GameDataManager.instance.currentlyPlayingLevelIndex = ld.levelIndex - 1;
        GameDataManager.instance.zenSceneDataManager.LoadingScene = ld.levelData;
        GameDataManager.instance.zenSceneDataManager.isLoad = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
