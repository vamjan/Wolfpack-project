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
    public class ObjectSpawner : MonoBehaviour
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

        //set reference to event manager
        private void SetInitialReference()
        {
            Debug.Log("Setting reference to gamemaster");
            eventMasterScript = GameObject.Find("GameManager").GetComponent<LevelManager>();
        }

        //spawn an object
        private void Spawn()
        {
            Debug.Log("Spawner doing its thing");
            //create new instace of given object
            Instantiate(spawnerType, this.GetComponent<Transform>().position, Quaternion.identity);
            if (!isPersistent)
                Destroy(this.gameObject);
        }
    }
}
