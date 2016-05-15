using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Wolfpack.Managers;

namespace Wolfpack
{
    /// <summary>
    /// Spawner script used to spawn gameobjects while the game is running.
    /// </summary>
    [System.Serializable]
    public class ObjectSpawnerScript : MonoBehaviour
    {

        public GameObject spawnerType;
        public bool isPersistent;

        private LevelManager eventMasterScript;

        //subscribe to spawn event
        void OnEnable()
        {
            SetInitialReference();
            eventMasterScript.SpawnEvent += Spawn;
        }

        //cancel subscribtion
        void OnDisable()
        {
            eventMasterScript.SpawnEvent -= Spawn;
        }

		/// <summary>
		/// Set initial references to neccesary objects and validate data.
		/// </summary>
        private void SetInitialReference()
        {
            Debug.Log("Setting reference to gamemaster");
            eventMasterScript = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }

		/// <summary>
		/// Spawn NPC into NPCs gameobject.
		/// Creates new object from prefab.
		/// </summary>
        private void Spawn()
        {
            Debug.Log("Spawner doing its thing");
            //create new instace of given object
			var obj = (GameObject)Instantiate(spawnerType, this.GetComponent<Transform>().position, Quaternion.identity);
			obj.transform.parent = GameObject.Find("NPCs").transform;
            if (!isPersistent)
                Destroy(this.gameObject);
        }
    }
}
