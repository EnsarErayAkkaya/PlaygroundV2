using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    Volume volume;

    TrainManager trainManager;
    public Camera rtsCamera;
    public Camera trainViewCamera;
    public Camera izoCamera;
    public Camera freeFlyCamera;
    
    ViewStyle[] styles = { ViewStyle.RTS, ViewStyle.TRAINVIEW, ViewStyle.FREEFLY, ViewStyle.IZO };
    ViewStyle selectedStyle = new ViewStyle();
    private int activeIndex;
    public Camera activeCamera;

    private void Start() 
    {
        UpdatePostProccessing();
        trainManager = FindObjectOfType<TrainManager>();    
    }
    void UpdatePostProccessing()
    {
        volume = FindObjectOfType<Volume>();
        if (SaveAndLoadGameData.instance.savedData.postProccessing)
        {
            volume.enabled = true;
        }
        else
        {
            volume.enabled = false;
        }
    }

    public void ChangeStyle()
    {
        activeIndex++;
        if( activeIndex >= styles.Length )
        {
            activeIndex = 0;
        }
        selectedStyle = styles[activeIndex];
        switch (selectedStyle)
        {
            case ViewStyle.RTS:
                rtsCamera.transform.parent.gameObject.SetActive(true);
                trainViewCamera.gameObject.SetActive(false);
                izoCamera.gameObject.SetActive(false);
                freeFlyCamera.gameObject.SetActive(false);
                activeCamera = rtsCamera;
            break;
            case ViewStyle.IZO:
                rtsCamera.transform.parent.gameObject.SetActive(false);
                trainViewCamera.gameObject.SetActive(false);
                izoCamera.gameObject.SetActive(true);
                freeFlyCamera.gameObject.SetActive(false);
                activeCamera = izoCamera;
            break;
            case ViewStyle.FREEFLY:
                if (Application.platform == RuntimePlatform.Android)
                {
                    ChangeStyle();
                }
                else
                {
                    freeFlyCamera.transform.position = activeCamera.transform.position;
                    freeFlyCamera.transform.rotation = activeCamera.transform.rotation;
                    rtsCamera.transform.parent.gameObject.SetActive(false);
                    trainViewCamera.gameObject.SetActive(false);
                    freeFlyCamera.gameObject.SetActive(true);
                    izoCamera.gameObject.SetActive(false);
                    activeCamera = freeFlyCamera;
                }
            break;
            case ViewStyle.TRAINVIEW:
                if(trainManager.trains.Count > 0)
                {
                    trainViewCamera.GetComponent<OrbitCamera>().target = trainManager.trains[0].transform;
                    rtsCamera.transform.parent.gameObject.SetActive(false);
                    trainViewCamera.gameObject.SetActive(true);
                    izoCamera.gameObject.SetActive(false);
                    freeFlyCamera.gameObject.SetActive(false);
                    activeCamera = trainViewCamera;
                }
                else
                {
                    Debug.Log("no train");
                    ChangeStyle();
                }
            break;
            default:
                rtsCamera.gameObject.SetActive(true);
                trainViewCamera.gameObject.SetActive(false);
                izoCamera.gameObject.SetActive(false);
                activeCamera = rtsCamera;
            break;
        }
    }
}
public enum ViewStyle
{
    RTS, TRAINVIEW, IZO, FREEFLY
}
