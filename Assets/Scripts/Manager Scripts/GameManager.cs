using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {


    /// 
    /// EVENTS
    /// 
    /*public delegate void GameManagerEventHandler();
    public event GameManagerEventHandler MenuToggleEvent;
    public event GameManagerEventHandler UIToggleEvent;
    public event GameManagerEventHandler GameOverEvent;
    public event GameManagerEventHandler PauseEvent;

    public bool isGameOver;
    public bool isMenuOn;*/


    //static instance of GameManager, which allows it to be accessed by any other script
    public static GameManager instance = null;
    //private reference to the character script for making calls to the public api.
    private PlayerCharacter character;
    //reference to the camera
    private Camera mainCamera;

    public ObjectSpawner[] spawners;

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

    void Awake()
    {
        GetGameManager();
        spawners = FindObjectsOfType(typeof(ObjectSpawner)) as ObjectSpawner[];
        Debug.Log("Manager running");
    }

    // Use this for initialization
    void Start () {
        foreach(var spawner in spawners)
        {
            spawner.spawn();
        }

        InputWrapper.inputEnabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
