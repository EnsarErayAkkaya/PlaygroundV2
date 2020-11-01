using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavbarUIManager : MonoBehaviour
{
    GameUIManager uIManager;
    [SerializeField] Button railsContentButton, envsContentButton, trainsContentButton;
    [SerializeField] ScrollRect contentScrollRect;

    [SerializeField] Transform railsContent, envsContent, trainsContent;
    [SerializeField] Image navbarImage;
    public RectTransform navbarTransform;

    [Header("Navbar Poses")]

    [SerializeField] Vector3 hidePos;
    [SerializeField] Vector3 showPos;

    [Header("Content Colors")]

    [SerializeField] Color railColor;
    [SerializeField] Color envColor;
    [SerializeField] Color trainColor;

    bool navbarShowing;

    void Start()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        uIManager = FindObjectOfType<GameUIManager>();

        navbarTransform.anchoredPosition = hidePos;

        // Clean
        foreach (Transform child in railsContent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in envsContent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in trainsContent)
        {
            Destroy(child.gameObject);
        }

        if(levelManager == null)
        {
            // fill
            foreach (var item in SaveAndLoadGameData.instance.savedData.playerRails)
            {
                RailData data = GameDataManager.instance.allRails.Find(s => s.railType == item);
                GameObject e = Instantiate(data.railButton);
                e.transform.SetParent(railsContent, false);
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.RailButtonClick(data.railPrefab, data.cost); } );
            }
            foreach (var item in SaveAndLoadGameData.instance.savedData.playerEnvs)
            {
                EnvironmentData data = GameDataManager.instance.allEnvs.Find(s => s.envType == item);
                GameObject e = Instantiate(data.envButton);
                e.transform.SetParent(envsContent, false);
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.EnvironmentCreateButtonClick(data.envPrefab, data.cost); } );
            }
            foreach (var item in SaveAndLoadGameData.instance.savedData.playerTrains)
            {
                TrainData data = GameDataManager.instance.allTrains.Find(s => s.trainType == item);
                GameObject e = Instantiate(data.trainButton);
                e.transform.SetParent(trainsContent, false);
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.TrainCreateButtonClick(data.trainPrefab, data.cost); } );
            }
        }
        else
        {
            // fill
            foreach (var item in levelManager.levelRails)
            {
                RailData data = GameDataManager.instance.allRails.Find(s => s.railType == item);
                GameObject e = Instantiate(data.railButton);
                e.transform.SetParent(railsContent, false);
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.RailButtonClick(data.railPrefab, data.cost); } );
            }
            foreach (var item in levelManager.levelEnvs)
            {
                EnvironmentData data = GameDataManager.instance.allEnvs.Find(s => s.envType == item);
                GameObject e = Instantiate(data.envButton);
                e.transform.SetParent(envsContent, false);
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.EnvironmentCreateButtonClick(data.envPrefab, data.cost); } );
            }
            foreach (var item in levelManager.levelTrains)
            {
                TrainData data = GameDataManager.instance.allTrains.Find(s => s.trainType == item);
                GameObject e = Instantiate(data.trainButton);
                e.transform.SetParent(trainsContent, false);
                e.GetComponent<Button>().onClick.AddListener( delegate{ uIManager.TrainCreateButtonClick(data.trainPrefab, data.cost); } );
            }
        }
    }
    public void Activete()
    {
        navbarTransform.gameObject.SetActive(true);
    }
    public void DeActivate()
    {
        navbarTransform.gameObject.SetActive(false);
    }

    public void RailsContentButtonClick()
    {
        ShowNavbar();

        navbarImage.color = railColor;

        railsContent.gameObject.SetActive(true);

        contentScrollRect.content = railsContent.GetComponent<RectTransform>();
        
        envsContent.gameObject.SetActive(false);
        trainsContent.gameObject.SetActive(false);
    }
    public void EnvsContentButtonClick()
    {
        ShowNavbar();

        navbarImage.color = envColor;

        envsContent.gameObject.SetActive(true);

        contentScrollRect.content = envsContent.GetComponent<RectTransform>();

        railsContent.gameObject.SetActive(false);
        trainsContent.gameObject.SetActive(false);
    }
    public void TrainsContentButtonClick()
    {
        ShowNavbar();

        navbarImage.color = trainColor;

        trainsContent.gameObject.SetActive(true);

        contentScrollRect.content = trainsContent.GetComponent<RectTransform>();

        envsContent.gameObject.SetActive(false);
        railsContent.gameObject.SetActive(false);
    }
    public void ShowNavbar()
    {
        navbarShowing = true;
        RefreshPosition();
    }
    public void HideNavbar()
    {
        navbarShowing = false;
        RefreshPosition();        
    }
    void RefreshPosition()
    {
        if (navbarShowing)
        {
            //StartCoroutine( RefreshPos(showPos) );
            navbarTransform.anchoredPosition = showPos;
        }
        else
        {
           //StartCoroutine( RefreshPos(hidePos) );
           navbarTransform.anchoredPosition = hidePos;
        }
    }
    /* IEnumerator RefreshPos(Vector3 pos)
    {
       
        float t = 0;
        while(t < .3f)
        {
            t += Time.deltaTime;
            navbarTransform.anchoredPosition = Vector3.Lerp( navbarTransform.anchoredPosition, pos, t );
            yield return null;
        }
    } */
}
