using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Wolfpack
{
    public class MainMenuScript : MonoBehaviour
    {

        public GameObject loadingImage;

        public void LoadScene(int level)
        {
            loadingImage.SetActive(true);
            SceneManager.LoadScene(level);
        }
    }
}
