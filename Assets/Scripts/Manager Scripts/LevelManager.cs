using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Wolfpack.Characters;
using UnityEngine.SceneManagement;

namespace Wolfpack.Managers
{
	/// <summary>
	/// Class of Level Manager to take care of level scripting. Controls all NPCs, triggers and spawners.
	/// </summary>
    public class LevelManager : MonoBehaviour
    {

        /// 
        /// EVENTS
        /// 
        public delegate void LevelManagerEventHandler();
        public event LevelManagerEventHandler SpawnEvent;
        public event LevelManagerEventHandler LevelFinnishedEvent;

		//Array of spawners. Setup by UnityEditor.
        public GameObject[] spawners;
		//Array of triggers. Setup by UnityEditor.
        public Collider2D[] triggers;
		//List of targetable NPCs.
        public List<GameObject> Targetables = new List<GameObject>();
		//TODO: GameObject is sometimes not removed properly. Looks random. More research required.
		//Player reference
		public PlayerCharacterScript player = null;

		/// <summary>
		/// Calls the spawn event.
		/// </summary>
        private void CallSpawnEvent()
        {
            if (SpawnEvent != null)
            {
                SpawnEvent();
            }
			GetNPCs();
        }

		/// <summary>
		/// Calls the level finnished event.
		/// </summary>
        public void CallLevelFinnishedEvent()
        {
            if (LevelFinnishedEvent != null)
            {
                LevelFinnishedEvent();
            }
            LoadNextLevel();
        }

		/// <summary>
		/// Spawns the first batch of zombies in demo level
		/// </summary>
		public void SpawnFirst() {
			triggers[0].gameObject.SetActive(false);
			spawners[0].SetActive(true);
			CallSpawnEvent();

			StartCoroutine(DelayedSpawn(10f));
		}

		/// <summary>
		/// Spawns the second batch of zombies in demo level
		/// </summary>
		public void SpawnSecond() {
			triggers[1].gameObject.SetActive(false);
			spawners[3].SetActive(true);
			spawners[4].SetActive(true);

			CallSpawnEvent();
		}

		/// <summary>
		/// Coroutine for delayed spawn in the first batch.
		/// </summary>
		/// <param name="time">Delay</param>
		private IEnumerator DelayedSpawn(float time) {
			yield return new WaitForSeconds(time);

			spawners[1].SetActive(true);
			spawners[2].SetActive(true);

			CallSpawnEvent();
		}

		/// <summary>
		/// Loads the next level.
		/// </summary>
        private void LoadNextLevel()
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }

		/// <summary>
		/// Gets the NPCs from NPCs gameObject.
		/// </summary>
		private void GetNPCs() {
			var tmp = GameObject.Find("NPCs").GetComponent<Transform>();

			//adds targetable foes to level manager and setups layering
			foreach (Transform curr in tmp)
			{	
				GameObject obj = curr.gameObject;
				if(!Targetables.Contains(obj)) //only if it is not already there
					Targetables.Add(obj);

				//setup desired layer of NPC
				Renderer r = curr.GetComponent<Renderer>();
				r.sortingOrder = GetLayer(curr.position.y);
			}
		}

		/// <summary>
		/// Set initial references to neccesary objects and validate data.
		/// Executes initial layering of scenery, npcs and interactables.
		/// </summary>
        private void SetInitialReference()
        {
			player = GameObject.Find("Player").GetComponent<PlayerCharacterScript>();

            //set intial layering for all scenery elements
            var tmp = GameObject.Find("Scenery").GetComponent<Transform>();

            foreach (Transform curr in tmp)
            {
                Renderer r = curr.GetComponent<Renderer>();
                r.sortingOrder = GetLayer(curr.position.y);
            }
			//set intial layering for all interatables elements
			tmp = GameObject.Find("Interactables").GetComponent<Transform>();

			foreach (Transform curr in tmp)
			{
				Renderer r = curr.GetComponent<Renderer>();
				r.sortingOrder = GetLayer(curr.position.y);
			}

			GetNPCs();
        }

		/// <summary>
		/// Gets the desired layer of gameObject.
		/// Based on y value of position.
		/// </summary>
		/// <returns>The layer</returns>
		/// <param name="y">The y coordinate</param>
        public int GetLayer(float y)
        {
            return Mathf.FloorToInt(y) * -1;
        }

        void Awake()
        {
            SetInitialReference();
			Debug.Log("LevelManager Awake " + this.GetHashCode());
        }

        // Use this for initialization
        void Start()
        {
			Debug.Log("LevelManager Start " + this.GetHashCode());
            CallSpawnEvent();
        }
    }
}