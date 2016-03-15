﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    /// 
    /// EVENTS
    /// 
    public delegate void LevelManagerEventHandler();
    public event LevelManagerEventHandler SpawnEvent;

    public GameObject[] objects; //will be needed
    public Collider2D[] triggers; //maybe

    private GameManager eventMasterScript;

    private void CallSpawnEvent()
    {
        if (SpawnEvent != null)
        {
            SpawnEvent();
        }
    }

    private void SetInitialReference()
    {
        eventMasterScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Awake()
    {
        SetInitialReference();
        Debug.Log("Level manager running");
    }

    // Use this for initialization
    void Start()
    {
        CallSpawnEvent();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
