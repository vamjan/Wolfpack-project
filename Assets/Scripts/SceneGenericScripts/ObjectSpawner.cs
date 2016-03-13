using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectSpawner : MonoBehaviour {

    public GameObject spawnerType;
    public bool persistent;

    void Awake()
    {
        Debug.Log("Spawner active!");
    }

    public void spawn()
    {
        Instantiate(spawnerType, this.GetComponent<Transform>().position, Quaternion.identity);
        if (!persistent)
            Destroy(this.gameObject);
    }
}
