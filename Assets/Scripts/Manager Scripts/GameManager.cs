using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Wolfpack.Character;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Wolfpack.Managers
{
    /// <summary>
    /// Class of Game Manager to take care of game cycle. Also contains events for pausing and unpausing game and for menu control.
    /// Singleton design pattern.
    /// 
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

        [SerializeField]
        private GameObject menu;
        //static instance of GameManager, which allows it to be accessed by any other script
        public static GameManager instance = null;
        //private reference to the character script for making calls to the public api.
        [SerializeField]
        private PlayerCharacter player;
        //reference to the camera
        [SerializeField]
        private Camera mainCamera;

        //public GameObject loadingImage;

        public void LoadScene(int level)
        {
            Destroy(PlayerManager.instance.gameObject);
            Destroy(this.gameObject);
            SceneManager.LoadScene(level);
        }

        public void GetGameManager()
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

        public void CallGameStartEvent()
        {
            isGameOver = false;
            isPaused = false;
            isMenuActive = false;

            if (GameStartEvent != null)
            {
                GameStartEvent();
            }
        }

        public void CallGameOverEvent()
        {
            isGameOver = true;

            if (GameOverEvent != null)
            {
                GameOverEvent();
            }

            GameComplete("Game Over!");
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

        private void GameComplete(string msg)
        {
            //if game complete is called before Start() it will break the game and it has to be restarted
            Debug.Log(msg);
            CallPauseToggleEvent();
            CallMenuToggleEvent();
            GameObject.Find("InGameMenu/BackButton").SetActive(false);
            Text text = GameObject.Find("InGameMenu/Wolf").GetComponent<Text>();
            text.text = msg;
        }

        private void SetInitialReference()
        {
            player = GameObject.Find("Player").GetComponent<PlayerCharacter>();
            menu = GameObject.Find("InGameMenu");
        }

        void OnLevelWasLoaded(int level)
        {
            if (this != instance) return;
            else SetInitialReference();
        }

        void OnEnable()
        {
            player.OnDeath += CallGameOverEvent;
        }

        void OnDisable()
        {
            //not necessary
            /*
            player.OnDeath -= CallGameOverEvent;
            */
        }

        void Awake()
        {
            GetGameManager();
            Debug.Log("GameManager Awake");
            SetInitialReference();
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("GameManager Start");
            menu.SetActive(false);
            InputWrapper.inputEnabled = true;
            CallGameStartEvent();
        }

        // Update is called once per frame
        void Update()
        {

            if (InputWrapper.GetButtonDown("Pause"))
            {
                CallPauseToggleEvent();
                CallMenuToggleEvent();
            }

        }
    }
}
