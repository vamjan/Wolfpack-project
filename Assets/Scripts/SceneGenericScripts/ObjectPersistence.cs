using UnityEngine;
using System.Collections;

public class ObjectPersistence : MonoBehaviour {

	// Called before Start()
	void Awake () {
        DontDestroyOnLoad(gameObject);
	}
}
