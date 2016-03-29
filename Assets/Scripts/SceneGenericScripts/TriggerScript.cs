using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class ColliderEvent : UnityEvent<Collider2D> { }

public class TriggerScript : MonoBehaviour
{ 
    public ColliderEvent OnEnter;
    public ColliderEvent OnExit;

    void OnTriggerEnter2D (Collider2D c)
    {
        OnEnter.Invoke(c);
    }

    void OnTriggerExit2D (Collider2D c)
    {
        OnExit.Invoke(c);
    }
}