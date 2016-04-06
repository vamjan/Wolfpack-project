using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class ColliderEvent : UnityEvent<Collider2D> { }

public class TriggerScript : MonoBehaviour
{ 
    public ColliderEvent OnEnter;
    public ColliderEvent OnExit;
    public ColliderEvent OnStay;

    void OnTriggerEnter2D (Collider2D c)
    {
        OnEnter.Invoke(c);
    }

    void OnTriggerStay2D(Collider2D c)
    {
        OnStay.Invoke(c);
    }

    void OnTriggerExit2D (Collider2D c)
    {
        OnExit.Invoke(c);
    }
}