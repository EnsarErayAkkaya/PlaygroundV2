using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivesManager : MonoBehaviour
{
    PlaygroundManager playgroundManager;
    PlayGround p;
    
    [Header("baloons")]
    #region baloons
        public GameObject baloonPrefab;
        public int baloonCount;
        public float baloonStartingHeight;
    #endregion
    [Header("FireBaloons")]
    #region FireBaloons
        public GameObject fireBaloonPrefab;
        public int fireBaloonCount;
        public float fireBaloonStartingHeight;
    #endregion
    [Header("clouds")]
    #region clouds
        public GameObject cloudPrefab;
        public int cloudCount;
        public float cloudStartingHeight;
    #endregion
    void Start()
    {
        playgroundManager = FindObjectOfType<PlaygroundManager>();
        StartCoroutine( InitializeCoroutine() );
    }
    IEnumerator InitializeCoroutine()
    {
        yield return new WaitForSeconds(.3f);
        SetPlayGround();

        if(SaveAndLoadGameData.instance.savedData.interactibleContentValue >= 3)
            CreateBaloons();
        if(SaveAndLoadGameData.instance.savedData.interactibleContentValue >= 2)
            CreateFireBaloons();
        if (SaveAndLoadGameData.instance.savedData.interactibleContentValue >= 1)
            CreateClouds();
    }
    public void SetPlayGround()
    {
        p = playgroundManager.playground;
    }
    public void CreateClouds()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);
        for (int i = 0; i < cloudCount; i++)
        {
            Vector3 pos = new Vector3( Random.Range( (float)p.minX, (float)p.maxX ), cloudStartingHeight,  Random.Range( (float)p.minZ, (float)p.maxZ ));
            Cloud c = Instantiate(cloudPrefab,pos,Quaternion.identity).GetComponent<Cloud>();
            c.SetCloud(x,z,p);
            c.transform.parent = this.transform;
        }
    }
    public void CreateBaloons()
    {
        for (int i = 0; i < baloonCount; i++)
        {
            Vector3 pos = new Vector3( Random.Range( (float)p.minX, (float)p.maxX ), baloonStartingHeight,  Random.Range( (float)p.minZ, (float)p.maxZ ));
            Ballon b = Instantiate(baloonPrefab,pos,Quaternion.identity).GetComponent<Ballon>();
            b.SetBaloon(p);
            b.transform.parent = this.transform;
        }
    }
    public void CreateFireBaloons()
    {
        for (int i = 0; i < fireBaloonCount; i++)
        {
            Vector3 pos = new Vector3( Random.Range( (float)p.minX, (float)p.maxX ), fireBaloonStartingHeight,  Random.Range( (float)p.minZ, (float)p.maxZ ));
            FireBallon b = Instantiate(fireBaloonPrefab,pos,Quaternion.identity).GetComponent<FireBallon>();
            b.SetBaloon(p);
            b.transform.parent = this.transform;
        }
    }
}
