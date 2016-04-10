using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectSpawner : MonoBehaviour {

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
        Instantiate(spawnerType, this.GetComponent<Transform>().position, Quaternion.identity);
        if (!isPersistent)
            Destroy(this.gameObject);
    }
}
