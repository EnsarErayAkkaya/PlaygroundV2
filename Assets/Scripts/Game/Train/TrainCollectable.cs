using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCollectable : InteractibleBase
{       
    [SerializeField]
    private float trainStopDuration;
    LevelManager levelManager;
    TrainManager trainManager;
    private void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        trainManager = FindObjectOfType<TrainManager>();
        if(levelManager != null)
            isSelectable = false;
        else
        {
            colliders[0].enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(levelManager != null && trainManager.isStarted)
        {
            if(other.transform.GetComponent<Train>() != null)
            {
                OnCollision(other.transform.GetComponent<Train>());
            }
            else if(other.transform.parent != null && other.transform.parent.GetComponent<Train>() != null)
            {
                OnCollision(other.transform.parent.GetComponent<Train>());
            }
        }
    }
    void OnCollision(Train t)
    {
        levelManager.TrainCollect();
        
        t.StopTrain();
        
        StartCoroutine( Collect(t) );
    }
    IEnumerator Collect(Train tr)
    {
        float t = 0;
        while (t < trainStopDuration)
        {
            t += Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);
        tr.ResumeTrain();
    }
    public override void Destroy()
    {
        Destroy(gameObject);
    }
}
