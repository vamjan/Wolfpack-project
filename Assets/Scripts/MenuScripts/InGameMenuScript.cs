using UnityEngine;
using System.Collections;
using Wolfpack.Managers;

namespace Wolfpack
{
	/// <summary>
	/// Control game manager using ingame menu.
	/// </summary>
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
            manager.ReloadScene(1);
            BackToGame();
        }

        void OnEnable()
        {
            manager = GameManager.instance;
        }
    }
}