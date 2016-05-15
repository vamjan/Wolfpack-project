using UnityEngine;
using System.Collections;

namespace Wolfpack.Characters
{
    /// <summary>
    /// Simple dummy script which triggers dummy animations when attacked
    /// </summary>
    public class DummyScript : NPCharacter
    {
		/// <summary>
		/// Method needed to implement IAttackable. Invokes health changes.
		/// </summary>
		/// <param name="value">Change value</param>
		/// <param name="dmg">Dmg.</param>
        public override void UpdateHealth(int dmg)
        {
            base.UpdateHealth(dmg);
            cachedAnim.SetTrigger("Hit");
        }

		/// <summary>
		/// No staggering for dummy.
		/// </summary>
		public override void Stagger() { }

		/// <summary>
		/// Die in the specified time.
		/// This has to be a coroutine to wait for death animation to play.
		/// </summary>
		/// <param name="time">Time</param>
		public override IEnumerator Die(int time)
		{
			cachedAnim.SetTrigger("Die");
			levelManager.Targetables.Remove(this.gameObject);
			yield return new WaitForSeconds(time);
			Destroy(this.gameObject);
		}
    }
}
