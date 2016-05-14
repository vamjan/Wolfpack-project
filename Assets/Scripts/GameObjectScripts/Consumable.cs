using UnityEngine;
using System.Collections;
using Wolfpack.Characters;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Wolfpack.Managers;

namespace Wolfpack
{
    [System.Serializable]
    public struct CountedConsumables
    {
        public int count;
        public string name;
        public int damage;
        public int speed;
        public Sprite icon;
        public Target type;
		public GameObject prefab;
    }

    public abstract class Consumable
    {
        public string name;
        public Sprite icon;
        public Target type;

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

		public override string ToString()
		{
			return this.name;
		}


        public abstract void Use(GameObject target);
    }

    public class Knife : Consumable
    {
        public int damage;
        public float speed;
		public int timeToLive = 100;
		public GameObject knifePrefab;

        public override void Use(GameObject target)
        {
			target.GetComponent<Character>().Shoot(knifePrefab, damage, speed, timeToLive);
        }
    }

    public class Potion : Consumable
    {
        public int damage;

		//TODO: more potion effects, only healing considered so far

        public override void Use(GameObject target)
        {
			target.GetComponent<Character>().DrinkPotion(damage);
        }
    }
}
