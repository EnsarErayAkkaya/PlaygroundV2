using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    CameraManager cameraManagaer;
    public Camera snapCam;
    public string fileName;
    public int resWidth = 256;
    public int resHeight = 256;
    private void Awake() 
    {
        //snapCam = GetComponent<Camera>();
        cameraManagaer = GetComponent<CameraManager>();
        
        if( !System.IO.Directory.Exists(Application.persistentDataPath + "/Snapshots") )
        {
            System.IO.Directory.CreateDirectory( Application.persistentDataPath + "/Snapshots" );
        }

        if(snapCam.targetTexture == null)
        {
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);
        }
        else
        {
            resWidth = snapCam.targetTexture.width;
            resHeight = snapCam.targetTexture.height;
        }

        snapCam.gameObject.SetActive(false);
    }
    public void TakeScreenshot()
    {   
        snapCam.transform.position = cameraManagaer.activeCamera.transform.position;
        snapCam.transform.rotation = cameraManagaer.activeCamera.transform.rotation;

        if(cameraManagaer.activeCamera.orthographic)
        {
            snapCam.orthographic = true;
            snapCam.orthographicSize = cameraManagaer.activeCamera.orthographicSize;
        }
        else
        {
            snapCam.orthographic = false;
            snapCam.fieldOfView = cameraManagaer.activeCamera.fieldOfView;
        }
        
        snapCam.gameObject.SetActive(true);
    }
    private void LateUpdate() 
    {
        if(snapCam.gameObject.activeInHierarchy)
        {
            Texture2D snapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            snapShot.ReadPixels( new Rect( 0, 0, resWidth, resHeight ), 0, 0 );
            byte[] bytes = snapShot.EncodeToPNG();
            fileName = SnapshotName();
            System.IO.File.WriteAllBytes(fileName, bytes);
            Debug.Log("ss saved");
            snapCam.gameObject.SetActive(false);
            OnSnapshotTaken();
        }
    }
    private string SnapshotName()
    {
        return string.Format("{0}/Snapshots/snap_{1}x{2}_{3}.png",
            Application.persistentDataPath,
            resWidth,
            resHeight,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
    private void OnSnapshotTaken()
    {   
        GameDataManager.instance.zenSceneDataManager.SaveZenSceneData( fileName );
    }
}
