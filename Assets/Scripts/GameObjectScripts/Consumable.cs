using UnityEngine;
using System.Collections;
using Wolfpack.Character;

namespace Wolfpack
{
    [SerializeField]
    public class Consumable
    {
        public string name;
        public int count;
        public int damage;
        public int speed;
        public Texture2D icon;

        public enum Target { SELF, ENEMY };

        public void Use(GameObject target)
        {

        }
    }
}
