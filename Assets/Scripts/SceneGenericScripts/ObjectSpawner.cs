using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectSpawner : MonoBehaviour {

    public GameObject spawnerType;
    public bool isPersistent;

    private LevelManager eventMasterScript;

    void OnEnable()
    {
        SetInitialReference();
        eventMasterScript.SpawnEvent += Spawn;
    }

    void OnDisable()
    {
        eventMasterScript.SpawnEvent -= Spawn;
    }

    private void SetInitialReference()
    {
        Debug.Log("Setting reference to gamemaster");
        eventMasterScript = GameObject.Find("GameManager").GetComponent<LevelManager>();
    }

    private void Spawn()
    {
        Debug.Log("Spawner doing its thing");
        Instantiate(spawnerType, this.GetComponent<Transform>().position, Quaternion.identity);
        if (!isPersistent)
            Destroy(this.gameObject);
    }
}
