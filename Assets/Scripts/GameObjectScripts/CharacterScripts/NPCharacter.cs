using UnityEngine;
using System.Collections;
using System;

namespace Wolfpack.Characters
{
    public class NPCharacter : Character, IKillable
    {
		/// <summary>
		/// Die in the specified time.
		/// This has to be a coroutine to wait for death animation to play.
		/// </summary>
		/// <param name="time">Time</param>
        public virtual IEnumerator Die(int time)
        {
			target = null;
            cachedAnim.SetTrigger("Die");
			levelManager.Targetables.Remove(this.gameObject);
			StartCoroutine(PauseMovement(time));
            yield return new WaitForSeconds(time);
			Destroy(this.gameObject);
        }

		/// <summary>
		/// Method needed to implement IAttackable. Invokes health changes.
		/// Check for death. NPCs have long death lingering in demo.
		/// </summary>
		/// <param name="value">Change value</param>
		public override void UpdateHealth(int value)
		{
			ChangeHealth(value);
			//TODO: magic number
			if(isDead)
				StartCoroutine(Die(4));
		}
    }
}
