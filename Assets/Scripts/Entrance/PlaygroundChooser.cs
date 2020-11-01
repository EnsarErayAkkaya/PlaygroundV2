﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlaygroundChooser : MonoBehaviour
{
    [SerializeField] Transform scene;
    [SerializeField] float rotateSpeed;
    List<GameObject> playgrounds = new List<GameObject>();
    List<PlaygroundType> playgroundTypes = new List<PlaygroundType>();
    int index;
    void Start()
    {
        index = 0;
        // fill
        foreach (var item in SaveAndLoadGameData.instance.savedData.playerPlaygrounds)
        {
            PlaygroundData data = GameDataManager.instance.allPlaygrounds.Find(s => s.playgroundType == item);

            GameObject obj = Instantiate(data.playgroundGamePrefab);
            obj.transform.parent = scene;

            playgrounds.Add(obj);
            playgroundTypes.Add(item);

            obj.SetActive(false);
        }
        playgrounds[index].SetActive(true);
    }
    void OnDisable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    void OnEnable()
    {
        for (int i = 0; i < playgrounds.Count; i++)
        {
            if(i == index)
                playgrounds[i].SetActive(true);
            else
                playgrounds[i].SetActive(false);
        }
    }
    public void RightButton()
    {
        playgrounds[index].SetActive(false);
        index++;

        if (index == playgrounds.Count)
            index = 0;

        playgrounds[index].SetActive(true);
    }
    public void LeftButton()
    {
        playgrounds[index].SetActive(false);
        index--;

        if (index < 0)
            index = playgrounds.Count - 1;

        playgrounds[index].SetActive(true);
    }
    public void ChoosePlayground()
    {
        GameDataManager.instance.ChoosePlayground(playgroundTypes[index]);
    }
}
