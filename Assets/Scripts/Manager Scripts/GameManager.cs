using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Wolfpack.Characters;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
		/// (not used in any way yet, still hopeful I will find a way to use those)
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
        private PlayerCharacterScript player;

		/// <summary>
		/// Reloads the scene.
		/// Used for restarting.
		/// </summary>
		/// <param name="level">Level.</param>
        public void ReloadScene(int level)
        {
            Destroy(PlayerManager.instance.gameObject);
            Destroy(this.gameObject);
            SceneManager.LoadScene(level);
        }

		/// <summary>
		/// Calls the game start event.
		/// </summary>
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

		/// <summary>
		/// Calls the game over event.
		/// </summary>
        public void CallGameOverEvent()
        {
            if (GameOverEvent != null)
            {
                GameOverEvent();
            }

            GameComplete("Game Over!");
        }

		/// <summary>
		/// Calls the game finished event.
		/// </summary>
		public void CallGameFinishedEvent()
		{
			if (GameOverEvent != null)
			{
				GameOverEvent();
			}

			GameComplete("Game Finished!");
		}

		/// <summary>
		/// Calls the pause toggle event.
		/// </summary>
        public void CallPauseToggleEvent()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0.0f : 1.0f;
			InputWrapper.inputEnabled = !isPaused;

            if (PauseToggleEvent != null)
            {
                PauseToggleEvent();
            }
        }

		/// <summary>
		/// Calls the menu toggle event.
		/// </summary>
        public void CallMenuToggleEvent()
        {
            isMenuActive = !isMenuActive;
            menu.SetActive(isMenuActive);

            if (MenuToggleEvent != null)
            {
                MenuToggleEvent();
            }
        }

		/// <summary>
		/// Completes the game with custom message.
		/// Used for Game Over and Game Finished
		/// </summary>
		/// <param name="msg">Message</param>
        private void GameComplete(string msg)
        {
            //if game complete is called before Start() it will break the game and it has to be restarted
			if (!isGameOver) { //checking if game is already over (cant acces some buttons if it is)
				isGameOver = true;
				Debug.Log(msg);
				CallPauseToggleEvent();
				CallMenuToggleEvent();
				GameObject.Find("InGameMenu/BackButton").SetActive(false);
				Text text = GameObject.Find("InGameMenu/Wolf").GetComponent<Text>();
				text.text = msg;
			}
        }

		/// <summary>
		/// Gets the game manager.
		/// Enforces the singleton design pattern. Every scene has the same GameManager thanks to it.
		/// Saves gamedata when transitionig to next scene.
		/// </summary>
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

		/// <summary>
		/// Set initial references to neccesary objects and validate data.
		/// </summary>
        private void SetInitialReference()
        {
            player = GameObject.Find("Player").GetComponent<PlayerCharacterScript>();
            menu = GameObject.Find("InGameMenu");
        }

		/// <summary>
		/// Raises the level was loaded event.
		/// Reinitializes the manager for new scene when new scene is loaded.
		/// </summary>
		/// <param name="level">Level</param>
        void OnLevelWasLoaded(int level)
        {
			if (this != instance)
				return;
			SetInitialReference();
			OnEnable();
			Start();
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
			Debug.Log("GameManager Awake " + this.GetHashCode());
            SetInitialReference();
        }

        // Use this for initialization
        void Start()
        {
			Debug.Log("GameManager Start " + this.GetHashCode());
            menu.SetActive(false);
            InputWrapper.inputEnabled = true;
            CallGameStartEvent();
        }

        // Update is called once per frame
        void Update()
        {
			if(!isGameOver)
	            if (InputWrapper.GetButtonDown("Pause"))
	            {
	                CallPauseToggleEvent();
	                CallMenuToggleEvent();
	            }
        }
    }
}
