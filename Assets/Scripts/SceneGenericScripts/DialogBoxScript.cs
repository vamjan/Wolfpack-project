using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Wolfpack.Managers;

public class DialogBoxScript : MonoBehaviour {
	//message given by SignScript.
	public string[] textContent = null;
	//Background image of dialogBox. Given by UnityEditor.
    [SerializeField]
    private Image box = null;
	//Textbox of dialogBox. Given by UnityEditor.
    [SerializeField]
    private Text text = null;
	//index of actual message, used to index text from textContent
    private int textIndex = 0;

	/// <summary>
	/// Set initial references to neccesary objects and validate data.
	/// </summary>
    private void SetInitialReference()
    {
        if(box == null || text == null)
        {
            Debug.Log("DialogBox Error! Reference is missing!");
        }
    }

	/// <summary>
	/// Toggles the content of the text.
	/// </summary>
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

	//Initialize
	public void Awake()
    {
        SetInitialReference();
    }

	//Start working
    public void Start()
    {
		GameManager.instance.CallPauseToggleEvent();
        ToggleTextContent();
    }
}
