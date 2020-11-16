using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR || UNITY_STANDALONE
public class ButtonLabel : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    Image labelBG;
    TextMeshProUGUI label;
    float fadeOutDuration = .5f;
    bool fadeIn;
    
    private void Start() 
    {
        labelBG = transform.GetChild(0).GetComponent<Image>();
        label = labelBG.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        OnPointerExitOrLeftUI();
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        OnPointerHoverOrHoldUI();
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        OnPointerExitOrLeftUI();
    }

    public void OnPointerHoverOrHoldUI()
    {
        fadeIn = true;
        StartCoroutine( FadeIn() );
    }
    public void OnPointerExitOrLeftUI()
    {
        fadeIn = false;
        StartCoroutine( FadeOut() );
    }

    IEnumerator FadeIn()
    {
        label.gameObject.SetActive(true);
        labelBG.gameObject.SetActive(true);
        while (label.color.a < 1.0f)
        {
            label.color = new Color(label.color.r, label.color.g, label.color.b, label.color.a + (Time.deltaTime / fadeOutDuration));
            
            if(!fadeIn)
                break;

            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        while (label.color.a > 0.0f)
        {
            label.color = new Color(label.color.r, label.color.g, label.color.b, label.color.a - (Time.deltaTime / fadeOutDuration));
            
            if(fadeIn)
                break;
            
            yield return null;
        }
        label.gameObject.SetActive(false);
        labelBG.gameObject.SetActive(false);
    }
}
#elif UNITY_ANDROID
public class ButtonLabel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Image labelBG;
    TextMeshProUGUI label;
    float fadeOutDuration = .5f;
    bool fadeIn;
    
    private void Start() 
    {
        labelBG = transform.GetChild(0).GetComponent<Image>();
        label = labelBG.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        OnPointerExitOrLeftUI();
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        OnPointerHoverOrHoldUI();
    }
    public void OnPointerUp(PointerEventData eventData) 
    {
        OnPointerExitOrLeftUI();
    }

    public void OnPointerHoverOrHoldUI()
    {
        fadeIn = true;
        StartCoroutine( FadeIn() );
    }
    public void OnPointerExitOrLeftUI()
    {
        fadeIn = false;
        StartCoroutine( FadeOut() );
    }

    IEnumerator FadeIn()
    {
        label.gameObject.SetActive(true);
        labelBG.gameObject.SetActive(true);
        while (label.color.a < 1.0f)
        {
            label.color = new Color(label.color.r, label.color.g, label.color.b, label.color.a + (Time.deltaTime / fadeOutDuration));
            
            if(!fadeIn)
                break;

            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        while (label.color.a > 0.0f)
        {
            label.color = new Color(label.color.r, label.color.g, label.color.b, label.color.a - (Time.deltaTime / fadeOutDuration));
            
            if(fadeIn)
                break;
            
            yield return null;
        }
        label.gameObject.SetActive(false);
        labelBG.gameObject.SetActive(false);
    }
}
#endif
