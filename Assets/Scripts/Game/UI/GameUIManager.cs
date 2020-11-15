using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("Referances")]
    public GameControllUI gameControlUI;
    public StartEndToggles startEndToggles;
    public LevelCreationManager levelCreationManager;

    #region privateReferances
    ObjectChooser objectChooser;
    RailManager railManager;
    EnvironmentManager environmentManager;
    RailWayChooser railWayChooser;
    TrainManager trainManager;
    ObjectPlacementManager placementManager;
    CameraManager cameras;
    NavbarUIManager navbarUI;
    LevelUI levelUI;
    ScreenshotHandler screenshotHandler;
    CollectableManager collectableManager;
    #endregion

    [Header("Data")]

    [SerializeField] Button changeRailWayButton;
    [SerializeField] Button moveButton, setConnectionButton, deleteButton, rotateButton, saveButton
        , levelSaveButton, playStopButton, trainSpeedButton, camerasButton, cleanButton, flipButton, pauseButton, resetButton ;
    [SerializeField] Image playImage, stopImage;

    public GameObject levelCreatingPanel;

    public bool buttonLock = false, levelCreatingMode = false;
    
    bool isPlaying, isMultiple;
    InteractibleBase interactible;
    List<InteractibleBase> interactibles;
    
    void Start()
    {
        if(FindObjectsOfType<Train>().Length > 0)
        {
            playStopButton.gameObject.SetActive(true);
        }

        railManager = FindObjectOfType<RailManager>();
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        objectChooser = FindObjectOfType<ObjectChooser>();
        cameras = FindObjectOfType<CameraManager>();
        trainManager = FindObjectOfType<TrainManager>();
        navbarUI = FindObjectOfType<NavbarUIManager>();
        railWayChooser = FindObjectOfType<RailWayChooser>();
        environmentManager = FindObjectOfType<EnvironmentManager>();
        screenshotHandler = FindObjectOfType<ScreenshotHandler>();
        collectableManager = FindObjectOfType<CollectableManager>();
        
        if(FindObjectOfType<LevelUI>() != null)
        {
            levelUI = FindObjectOfType<LevelUI>();
            saveButton.gameObject.SetActive(false);
            gameControlUI.SetIsLevel();
        }
    }
    public void DeleteButtonClick()
    {
        if(interactible == null || buttonLock)
            return;

        PlayButtonSound();
        
        if(isMultiple)
        {
            foreach (var item in interactibles)
            {
                item.Destroy();
            }
        }
        else
        {
            interactible.Destroy();

            objectChooser.Unchoose();
            SetInteractible(null);

            if( trainManager.trains.Count <= 0 )
            {
                playStopButton.gameObject.SetActive(false);
            }
        }
    }
    public void RotateButtonClick()
    {
        if(interactible == null || buttonLock)
            return;

        PlayButtonSound();

        if(isMultiple)
        {
            foreach (var item in interactibles)
            {
                railManager.RotateRail(item.GetComponent<Rail>());
            }
        }
        else
        {
            if(interactible.GetComponent<Rail>() != null)
                railManager.RotateRail(interactible.GetComponent<Rail>());
            else if(interactible.GetComponent<EnvironmentObject>() != null)
                environmentManager.RotateEnv(interactible.GetComponent<EnvironmentObject>());
            else if(interactible.GetComponent<TrainCollectable>() != null)
                collectableManager.Rotate(interactible.gameObject);
        }
    }
    public void FlipRailButtonClick()
    {
        if(buttonLock) return;

        PlayButtonSound();

        if( interactible != null && interactible.GetComponent<Rail>() != null)
            railManager.FlipRail(interactible.GetComponent<Rail>());
    }
    public void RailButtonClick(GameObject obj, int cost)
    {
        if(buttonLock) return;

        PlayButtonSound();
        if( levelUI != null && levelUI.SetBudget(-cost))
        {
            buttonLock = true;
            if( interactible != null && interactible.GetComponent<Rail>() != null)
                railManager.NewRailConnection(interactible.GetComponent<Rail>(), obj, cost);
            else
                railManager.CreateFloatingRail(obj, cost);
        }
        else if( levelUI == null )
        {
            buttonLock = true;
            if( interactible != null && interactible.GetComponent<Rail>() != null)
                railManager.NewRailConnection(interactible.GetComponent<Rail>(), obj, cost);
            else
                railManager.CreateFloatingRail(obj, cost);
        }
    }
    public void EnvironmentCreateButtonClick(GameObject obj, int cost)
    {
        if(buttonLock) return;
        
        PlayButtonSound();

        if( levelUI != null && levelUI.SetBudget(-cost))
        {
            buttonLock = true;
            environmentManager.CreateEnvironmentObject(obj, cost);
        }
        else if( levelUI == null )
        {
            buttonLock = true;
            environmentManager.CreateEnvironmentObject(obj, cost);
        }
    }
    public void TrainCreateButtonClick(GameObject train, int cost)
    {
        if(interactible == null ||interactible.GetComponent<Rail>() == null || buttonLock)
            return;

        PlayButtonSound();    
            
        if( levelUI != null && levelUI.SetBudget(-cost))
        {
            buttonLock = true;
            trainManager.CreateTrain(interactible.GetGameObject(), train, cost);

            if( trainManager.trains.Count > 0 )
            {
                playStopButton.gameObject.SetActive(true);
            }
        }
        else if( levelUI == null )
        {
            buttonLock = true;  
            trainManager.CreateTrain(interactible.GetGameObject(), train, cost);

            if( trainManager.trains.Count > 0 )
            {
                playStopButton.gameObject.SetActive(true);
            }
        }
        buttonLock = false;   
    }
    public void CreateCollectableButton()
    {
        if(buttonLock)
            return;
        
        PlayButtonSound();

        collectableManager.CreateCollectable();
    }
    public void ChangeRailWayButtonClick()
    {
        if(interactible == null || buttonLock)
            return;

        PlayButtonSound();
        
        if( interactible.GetComponent<Rail>() != null )
        {
            buttonLock = true;
            railWayChooser.ChangeRailway(interactible.GetComponent<Rail>());
        }
        buttonLock = false; 
    }
    public void MoveButtonClick()
    {
        if(interactible == null || buttonLock)
            return;

        PlayButtonSound();    

        if( isMultiple )
        {   
            buttonLock = true;
            placementManager.PlaceMe(objectChooser.multipleObjectParent.gameObject, PlacementType.RailSystem);
        }
    }
    public void SetConnectionButtonClick()
    {
        if(interactible == null || buttonLock)
            return;

        PlayButtonSound();
        
        buttonLock = true;

        railManager.ExistingRailConnection(interactible.GetComponent<Rail>());
    }
    public void SetTrainSpeedButtonClick()
    {
        PlayButtonSound();

        trainManager.ChangeSpeed();
    }
    public void SaveButtonClick()
    {
        PlayButtonSound();

        screenshotHandler.TakeScreenshot();
    }
    public void SaveLevelButtonClick()
    {
        PlayButtonSound();

        GameDataManager.instance.zenSceneDataManager.SaveLevelSceneData(levelCreationManager.GetLevelContentData());
    }
    public void SetCamerasButton()
    {
        PlayButtonSound();

        cameras.ChangeStyle();
    }
    public void SetTrainsBackButton()
    {
        if(buttonLock) return;

        PlayButtonSound();
    
        trainManager.isStarted = false;

        isPlaying = false;

        navbarUI.Activete();
        
        isPlaying = false;
        trainManager.StopAllTrains();
        playImage.gameObject.SetActive(true);
        stopImage.gameObject.SetActive(false);

        trainManager.ResetAllTrains();
        SetUI(null);
    }
    public void PlayStopButtonClick()
    {
        if(buttonLock) return;

        PlayButtonSound();

        if(isPlaying == false && trainManager.isStarted == false )
        {
            OnNavbarDeactive(); // activate if buttons are disabled

            changeRailWayButton.GetComponent<RectTransform>().localPosition = Vector2.zero;
            // butonları gizle
            deleteButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            setConnectionButton.gameObject.SetActive(false);
            flipButton.gameObject.SetActive(false);

            //navbarı gizle
            navbarUI.DeActivate();

            if( levelUI == null )
                saveButton.gameObject.SetActive(true);
        }
        if(isPlaying)
        {
            isPlaying = false;
            trainManager.StopAllTrains();
            playImage.gameObject.SetActive(true);
            stopImage.gameObject.SetActive(false);
        }
        else
        {
            playStopButton.targetGraphic = stopImage;
            isPlaying = true;
            trainManager.StartTrains();
            playImage.gameObject.SetActive(false);
            stopImage.gameObject.SetActive(true);
        }
        SetUI(null);
    }

    public void ToggleLevelCreationUI()
    {
        // toggle level creting stuff
        if(levelCreatingMode == true) // mode active
        {
            levelCreatingMode = false;
            // disable mode
            levelCreatingPanel.SetActive(false);
            saveButton.gameObject.SetActive(true);
        }
        else
        {
            levelCreatingMode = true;
            // enable mode
            levelCreatingPanel.SetActive(true);
            saveButton.gameObject.SetActive(false);
        }
    }

    void PlayButtonSound()
    {
        AudioManager.instance.Play("ButtonClick");
    }

    public void OnNavbarActive()
    {
        camerasButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
    }
    public void OnNavbarDeactive()
    {
        camerasButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(true);
    }

    public void SetInteractible(GameObject obj)
    {
        try
        {
            if(interactible != null)
            {
                if(interactible.GetGameObject().GetComponent<Rail>() != null)
                {
                    
                    if( interactible.GetComponent<Rail>().GetOutputConnectionPoints().Length > 1 )
                    {
                        interactible.GetComponent<SplineManager>().HideTracks();
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

        SetUI(obj);
    }
    public void SetUI(GameObject obj)
    {
        isMultiple = false;
        moveButton.gameObject.SetActive(false);
        if(obj == null)
        {
            buttonLock = false;
            interactible = null;
            interactibles = null;

            deleteButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            setConnectionButton.gameObject.SetActive(false);
            changeRailWayButton.gameObject.SetActive(false);
            flipButton.gameObject.SetActive(false);

            if(levelUI == null)
                startEndToggles.HideToggles();

            if(trainManager.isStarted)
            {
                //navbarı gizle
                navbarUI.DeActivate();
                //trainSpeedButton.gameObject.SetActive(true);
            }
            else
            {
                navbarUI.HideNavbar();
            }
        }
        else if(obj != null && !trainManager.isStarted)
        {         
            interactible = obj.GetComponent<InteractibleBase>();

            navbarUI.Activete();//navbarı aç

            if(interactible.isStatic)
            {
                deleteButton.gameObject.SetActive(false);
                rotateButton.gameObject.SetActive(false);
                setConnectionButton.gameObject.SetActive(false);
                flipButton.gameObject.SetActive(false);

                if(interactible.GetComponent<Rail>() != null)
                {
                    Rail r = interactible.GetComponent<Rail>();
                    if(levelUI == null)
                        startEndToggles.SetToggles( r );

                    if(r.GetOutputConnectionPoints().Length > 1 )
                    {
                        changeRailWayButton.gameObject.SetActive(true);
                        r.splineManager.ShowTrack();
                    }
                    else
                    {
                        changeRailWayButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if(levelUI == null)
                        startEndToggles.HideToggles();
                    changeRailWayButton.gameObject.SetActive(false);
                }
            }
            else
            {
                deleteButton.gameObject.SetActive(true);
                rotateButton.gameObject.SetActive(true);
                
                if(interactible.GetComponent<Rail>() != null)
                {
                    Rail r = interactible.GetComponent<Rail>();
                    
                    flipButton.gameObject.SetActive(true);
                    
                    if(levelUI == null)
                        startEndToggles.SetToggles( r );

                    setConnectionButton.gameObject.SetActive(true);
                    if(r.GetOutputConnectionPoints().Length > 1 )
                    {
                        changeRailWayButton.gameObject.SetActive(true);
                        r.splineManager.ShowTrack();
                    }
                    else
                    {
                        changeRailWayButton.gameObject.SetActive(false);
                    }
                }
                else if( interactible.GetComponent<Train>() != null )
                {
                    if(levelUI == null)
                        startEndToggles.HideToggles();

                    deleteButton.gameObject.SetActive(true);

                    rotateButton.gameObject.SetActive(false);
                    setConnectionButton.gameObject.SetActive(false);
                    changeRailWayButton.gameObject.SetActive(false);
                    flipButton.gameObject.SetActive(false);
                }
                else
                {
                    if(levelUI == null)
                        startEndToggles.HideToggles();
                }
            }            
        }
        else if(obj != null && trainManager.isStarted)
        {
            // tren başlamışsa
            interactible = obj.GetComponent<InteractibleBase>();
            
            if(interactible.GetComponent<Rail>() != null)
            {
                Rail r = interactible.GetComponent<Rail>();

                if(levelUI == null)
                    startEndToggles.SetToggles( r );

                if(r.GetOutputConnectionPoints().Length > 1 )
                {
                    changeRailWayButton.gameObject.SetActive(true);
                    r.splineManager.ShowTrack();
                }
                else
                {
                    changeRailWayButton.gameObject.SetActive(false);
                }
            }
            else
            {
                if(levelUI == null)
                    startEndToggles.HideToggles();
            }
        }
    }
    public void SetUIMultiple( List<InteractibleBase> objects )
    {
        if(objects != null)
        {
            interactibles = objects;

            isMultiple = true;

            //navbarUI.gameObject.SetActive(false);// gizle
            if(!trainManager.isStarted)
            {
                deleteButton.gameObject.SetActive(true);
                moveButton.gameObject.SetActive(true);
            }

            rotateButton.gameObject.SetActive(false);
            changeRailWayButton.gameObject.SetActive(false);
            setConnectionButton.gameObject.SetActive(false);
            flipButton.gameObject.SetActive(false);
        }
        
    } 
    public GameObject GetChoosed()
    {
        return interactible.gameObject;
    }
}
