using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Wolfpack
{
    /// <summary>
    /// Serializable collider Unity event.
	/// Used in UnityEditor to setup the trigger.
    /// </summary>
    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider2D> { }

	/// <summary>
	/// Generic trigger script. Can be used for any collision based event invokations.
	/// </summary>
    public class TriggerScript : MonoBehaviour
    {
        public ColliderEvent OnEnter;
        public ColliderEvent OnExit;
        public ColliderEvent OnStay;

        void OnTriggerEnter2D(Collider2D col)
        {
            OnEnter.Invoke(col);
        }

        void OnTriggerStay2D(Collider2D col)
        {
            OnStay.Invoke(col);
        }

        void OnTriggerExit2D(Collider2D col)
        {
            OnExit.Invoke(col);
        }
    }
}