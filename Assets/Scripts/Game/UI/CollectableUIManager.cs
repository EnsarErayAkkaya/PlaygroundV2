using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectableUIManager : MonoBehaviour
{
    [SerializeField] GameObject collectableUIPrefab;
    [SerializeField] Transform collectableUIParent;
    LevelManager levelManager;
    List<CollectableUIObject> collectableUIs = new List<CollectableUIObject>();
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        Set();
    }
    public void Set()
    {
        while (collectableUIs.Count > 0)
        {
            Destroy(collectableUIs[0].gameObject);
            collectableUIs.RemoveAt(0);
        }
        for (int i = 0; i < levelManager.collectableCount; i++)
        {
            collectableUIs.Add(Instantiate(collectableUIPrefab, collectableUIParent).GetComponent<CollectableUIObject>());
        }
    }
    public void OnCollected(int collectedCount)
    {
        collectableUIs[collectableUIs.Count - collectedCount].Fill();
    }

}
