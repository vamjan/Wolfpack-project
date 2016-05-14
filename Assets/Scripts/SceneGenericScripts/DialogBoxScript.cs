using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Wolfpack.Managers;

public class DialogBoxScript : MonoBehaviour {

    public string[] textContent = null;

    [SerializeField]
    private Image box;

    [SerializeField]
    private Text text;

    private int textIndex = 0;

    private void SetInitialReference()
    {
        if(box == null || text == null)
        {
            Debug.Log("DialogBox Error! Reference is missing!");
        }
    }

    public void ToggleTextContent()
	{
        if(textIndex < textContent.Length)
        {
            text.text = textContent[textIndex++];
        } else {
			GameManager.instance.CallPauseToggleEvent();
            Destroy(this.gameObject);
        }
    }

	public void Awake()
    {
        SetInitialReference();
    }

    public void Start()
    {
		GameManager.instance.CallPauseToggleEvent();
        ToggleTextContent();
    }
}
