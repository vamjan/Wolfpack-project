using UnityEngine;
using System.Collections;

namespace Wolfpack
{
    /// <summary>
    /// Script to simulate isometric behaviour. Since we are not using true isometric grid, some assets don't behave properly when
    /// characters move around them. This Script fixes it by adding a trigger collider which triggers layer corrections.
    /// </summary>
    public class IsometricTriggerScript : MonoBehaviour
    {

        public Renderer cachedRenderer;
        public int value;

        void OnTriggerEnter2D(Collider2D col)
        {
            if (cachedRenderer) cachedRenderer.sortingOrder -= value;
            else Debug.LogError("Isometric trigger " + this + " is missing its target!");
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (cachedRenderer) cachedRenderer.sortingOrder += value;
            else Debug.LogError("Isometric trigger " + this + " is missing its target!");
        }

        void Awake()
        {
            cachedRenderer = GetComponentInParent<Renderer>();
        }
    }
}
