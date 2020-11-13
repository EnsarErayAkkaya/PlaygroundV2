using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriterUI : MonoBehaviour
{
    public TextMeshProUGUI txt;
	public string story;
    public bool cancelWriting, writing;
    private void Start() {
        txt = GetComponent<TextMeshProUGUI> ();
    }
    public void CallPlayText()
    {
        cancelWriting = true;
		txt.text = "";
        StartCoroutine ("PlayText");
    }

	IEnumerator PlayText()
	{
        yield return new WaitForSeconds (0.5f);
        writing = true;
        cancelWriting = false;
		foreach (char c in story) 
		{
            if(cancelWriting)
            {
                cancelWriting = false;
                break;
            }
			txt.text += c;
            if(c != ' ')
			    yield return new WaitForSeconds (0.08f);
		}
        writing = false;
	}
}
