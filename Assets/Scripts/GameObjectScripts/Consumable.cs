using UnityEngine;
using System.Collections;
using Wolfpack.Characters;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Wolfpack.Managers;

namespace Wolfpack
{
	/// <summary>
	/// Counted consumables.
	/// Only to initialize the inventory (Unity does not like abstract classes).
	/// </summary>
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

	/// <summary>
	/// Consumable.
	/// Instances of this class are in actual inventory.
	/// Has custom implementation for dictionary sorting and searching (only by name).
	/// </summary>
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

		/// <summary>
		/// Owner of this instance will create new knife from prefab and throw it.
		/// </summary>
		/// <param name="target">Target - owner</param>
        public override void Use(GameObject target)
        {
			target.GetComponent<Character>().Shoot(knifePrefab, damage, speed, timeToLive);
        }
    }

    public class Potion : Consumable
    {
        public int damage;

		//TODO: more potion effects, only healing considered so far

		/// <summary>
		/// Owner of this instance will use this potion.
		/// </summary>
		/// <param name="target">Target - owner</param></param>
        public override void Use(GameObject target)
        {
			target.GetComponent<Character>().DrinkPotion(damage);
        }
    }
}
