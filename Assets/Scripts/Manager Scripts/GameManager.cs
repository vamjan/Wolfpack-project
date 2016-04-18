using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Wolfpack.Character;

namespace Wolfpack.Managers
{
    /// <summary>
    /// Class of Game Manager to take care of game cycle. Also contains events for pausing and unpausing game and for menu control.
    /// Singleton design pattern.
    /// </summary>
    public class GameManager : MonoBehaviour
    {


        /// 
        /// EVENTS
        /// 
        public delegate void GameManagerEventHandler();
        public event GameManagerEventHandler GameStartEvent;
        public event GameManagerEventHandler GameOverEvent;
        public event GameManagerEventHandler MenuToggleEvent;
        public event GameManagerEventHandler PauseToggleEvent;

        ///
        /// FLAGS
        ///
        private bool isGameOver;
        private bool isMenuActive;
        private bool isPaused;

        public GameObject menu;
        //static instance of GameManager, which allows it to be accessed by any other script
        public static GameManager instance = null;
        //private reference to the character script for making calls to the public api.
        private PlayerCharacter character;
        //reference to the camera
        private Camera mainCamera;

        void GetGameManager()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        void CallGameStartEvent()
        {
            isGameOver = false;
            isPaused = false;
            isMenuActive = false;

            if (GameStartEvent != null)
            {
                GameStartEvent();
            }
        }

        public void CallPauseToggleEvent()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0.0f : 1.0f;

            if (PauseToggleEvent != null)
            {
                PauseToggleEvent();
            }
        }

        public void CallMenuToggleEvent()
        {
            isMenuActive = !isMenuActive;
            menu.SetActive(isMenuActive);

            if (MenuToggleEvent != null)
            {
                MenuToggleEvent();
            }
        }

        void Awake()
        {
            GetGameManager();
            Debug.Log("Game manager running");
        }

        // Use this for initialization
        void Start()
        {
            InputWrapper.inputEnabled = true;
            CallGameStartEvent();
        }

        // Update is called once per frame
        void Update()
        {

            if (InputWrapper.GetButtonDown("Cancel"))
            {
                CallPauseToggleEvent();
                CallMenuToggleEvent();
                Debug.Log("Game is paused: " + isPaused);
            }

        }
    }
}
