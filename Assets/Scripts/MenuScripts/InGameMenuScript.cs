using UnityEngine;
using System.Collections;
using Wolfpack.Managers;

namespace Wolfpack
{
    public class InGameMenuScript : MonoBehaviour
    {

        private GameManager manager = null;

        public void Exit()
        {
            Application.Quit();
        }

        public void BackToGame()
        {
            manager.CallMenuToggleEvent();
            manager.CallPauseToggleEvent();
        }

        public void RestartGame()
        {
            manager.LoadScene(1);
            BackToGame();
        }

        void OnEnable()
        {
            manager = GameManager.instance;
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}