using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public Transform contentParent;
    public GameObject storePrefab;
    private EntranceUI entranceUI;
    private PlaygroundChooser playgroundChooser;
    public GameObject purchasedPanel;

    public PurchaseData purchaseData;

    public static string pandaCarpet = "pandacarpet";
    public static string lionCarpet = "lioncarpet";
    public static string voxTrain = "voxtrain";
    public static string woodTrain = "woodtrain";

    private void Start() {
        entranceUI = FindObjectOfType<EntranceUI>();
        playgroundChooser = FindObjectOfType<PlaygroundChooser>();
        SetStore();
    }
    void SetStore()
    {
        foreach (Transform item in contentParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in purchaseData.purchaseItems0) // first group items
        {
            GameObject g = Instantiate(storePrefab, contentParent);

            PurchaseObject po = g.GetComponent<PurchaseObject>();

            po.Set(item);

            if(!item.withLeaf)
                po.buy.onClick.AddListener(delegate { buy(item.name); });
            else
                po.buy.onClick.AddListener(delegate { buyWithLeaf(item.name, (int)item.cost); });
        }
        foreach (var item in purchaseData.purchaseItems) // second group
        {
            GameObject g = Instantiate(storePrefab,contentParent);

            PurchaseObject po = g.GetComponent<PurchaseObject>();
            
            po.Set(item);

            if (!item.withLeaf)
                po.buy.onClick.AddListener(delegate { buy(item.name); });
            else
                po.buy.onClick.AddListener(delegate { buyWithLeaf(item.name, (int)item.cost); });
        }
    }
    public void buy(string s)
    {
        FindObjectOfType<Purchaser>().BuyProductID(s);
    }
    
    public void buyWithLeaf(string s, int leafAmount)
    {
        if (SaveAndLoadGameData.instance.savedData.leaf - leafAmount > 0)
        {
            SaveAndLoadGameData.instance.savedData.leaf -= leafAmount;
            // PANDA CARPET
            if (String.Equals(s, pandaCarpet, StringComparison.Ordinal))
            {
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                GameDataManager.instance.AddNewPlayerPlayground(PlaygroundType.Panda);
                playgroundChooser.OnNewPlaygroundPurchased();
            }
            // LION CARPET
            else if (String.Equals(s, lionCarpet, StringComparison.Ordinal))
            {
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                GameDataManager.instance.AddNewPlayerPlayground(PlaygroundType.Lion);
                playgroundChooser.OnNewPlaygroundPurchased();
            }
            // VOX TRAIN
            else if (String.Equals(s, voxTrain, StringComparison.Ordinal))
            {
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                GameDataManager.instance.AddNewPlayerTrain(TrainType.Voxel);
            }
            // WOOD TRAIN
            else if (String.Equals(s, woodTrain, StringComparison.Ordinal))
            {
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                GameDataManager.instance.AddNewPlayerTrain(TrainType.Wooden);
            }
            SaveAndLoadGameData.instance.Save();
            OnPurchaseEnded();
        }
        else
        {
            Debug.Log("Not enough leaf.");
        }
    }
    void OnPurchaseEnded()
    {
        entranceUI.UpdateLeaftext();
        SetStore();
        ShowPurchaseCompletedPanel();
    }
    public void ShowPurchaseCompletedPanel()
    {
        purchasedPanel.gameObject.SetActive(true);
    }
}
