using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Wolfpack.Managers
{
    public class LevelManager : MonoBehaviour
    {

        /// 
        /// EVENTS
        /// 
        public delegate void LevelManagerEventHandler();
        public event LevelManagerEventHandler SpawnEvent;
        public event LevelManagerEventHandler LevelFinnishedEvent;

        public GameObject[] objects;  //will be needed
        public Collider2D[] triggers; //maybe
        public List<GameObject> Targetables = new List<GameObject>();

        private GameManager eventMasterScript;

        private void CallSpawnEvent()
        {
            if (SpawnEvent != null)
            {
                SpawnEvent();
            }
        }

        public void CallLevelFinnishedEvent()
        {
            if (LevelFinnishedEvent != null)
            {
                LevelFinnishedEvent();
            }
            LoadNextLevel();
        }

        private void LoadNextLevel()
        {
            throw new NotImplementedException();
        }

        private void SetInitialReference()
        {
            eventMasterScript = GameManager.instance;

            //set intial layering for all scenery elements
            var tmp = GameObject.Find("Scenery").GetComponent<Transform>();

            foreach (Transform curr in tmp)
            {
                Renderer r = curr.GetComponent<Renderer>();
                r.sortingOrder = GetLayer(curr.position.y);
            }


        }

        public int GetLayer(float y)
        {
            return Mathf.FloorToInt(y / 10) * -1;
        }

        void Awake()
        {
            SetInitialReference();
            Debug.Log("LevelManager Awake");
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("LevelManager Start");
            CallSpawnEvent();
        }
    }
}
