using UnityEngine;
using System.Collections;
using Wolfpack.Character;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

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

        public abstract void Use(GameObject target);
    }

    public class Knife : Consumable
    {
        public int damage;
        public int speed;

        public override void Use(GameObject target)
        {
            throw new NotImplementedException();
        }
    }

    public class Potion : Consumable
    {
        public int damage;

        public override void Use(GameObject target)
        {
            throw new NotImplementedException();
        }
    }
}
