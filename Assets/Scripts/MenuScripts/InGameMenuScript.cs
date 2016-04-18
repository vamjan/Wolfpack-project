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

        void OnEnable()
        {
            manager = GameManager.instance;
        }
    }
}