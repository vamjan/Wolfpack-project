using UnityEngine;
using System.Collections;
using Wolfpack.Characters;

namespace Wolfpack
{
    public class AttackHitboxScript : MonoBehaviour
    {
		//damage of hitbox
        public int damage;
		//owner of hitbox, used to get alegiance
		public Character owner;

		/// <summary>
		/// Raises the trigger enter2d event.
		/// </summary>
		/// <param name="col">Col</param>
        public virtual void OnTriggerEnter2D(Collider2D col)
        {
            Component tmp;
            Debug.Log("Collider hit something! " + col.ToString());
            if ((tmp = col.gameObject.GetComponent(typeof(IAttackable)) as Component) != null)
            {
				if(tmp is Character)
				{
					if (owner.al != (tmp as Character).al)
						(tmp as IAttackable).UpdateHealth(damage);
				} else {
					(tmp as IAttackable).UpdateHealth(damage);
				}
            }
        }
    }
}
