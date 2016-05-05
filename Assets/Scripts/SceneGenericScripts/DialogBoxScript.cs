using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

        if(textContent == null)
        {
            textContent = new string[] { "text text text text text text text text text text text text text text text text text",
            "moar text moar text moar text moar text moar text moar text"};
        }
    }

    public void ToggleTextContent()
    {
        if(textIndex < textContent.Length)
        {
            text.text = textContent[textIndex++];
        } else
        {
            Destroy(this.gameObject);
        }
    }

	public void Awake()
    {
        SetInitialReference();
    }

    public void Start()
    {
        ToggleTextContent();
    }
}
