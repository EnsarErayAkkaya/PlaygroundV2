using UnityEngine;
using UnityEngine.UI;
public class CollectableUIObject : MonoBehaviour
{
    [SerializeField] private GameObject fill, empty;
    public void Fill()
    {
        fill.SetActive(true);
        empty.SetActive(true);
    }
}
