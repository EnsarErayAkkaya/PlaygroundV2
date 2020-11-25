using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public Transform contentParent;
    public GameObject storePrefab;

    public PurchaseData purchaseData;

    private void Start() {
        SetScene();
    }
    void SetScene()
    {
        foreach (Transform item in contentParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in purchaseData.purchaseItems)
        {
            GameObject g = Instantiate(storePrefab,contentParent);

            PurchaseObject po = g.GetComponent<PurchaseObject>();
            
            po.Set(item);

            po.buy.onClick.AddListener( delegate{ buy(item.name); } );
        }
    }
    public void buy(string s)
    {
        FindObjectOfType<Purchaser>().BuyProductID(s);
    }
}
