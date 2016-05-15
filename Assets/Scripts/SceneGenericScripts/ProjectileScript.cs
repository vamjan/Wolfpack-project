using UnityEngine;
using System.Collections;
using Wolfpack.Characters;

namespace Wolfpack {
	/// <summary>
	/// Script attached to every projectile.
	/// </summary>
	public class ProjectileScript : AttackHitboxScript {

		//projectile speed
		public float velocity;
		//time left before destruction
		public int timeToLive;
		//cached renderer for spinning motion
		private SpriteRenderer cachedRenderer;

		/// <summary>
		/// Set initial references to neccesary objects and validate data.
		/// </summary>
		private void SetInitialReference() {
			cachedRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		/// <summary>
		/// Sets the owner.
		/// Unity does not like custom getters and setters :(
		/// </summary>
		/// <param name="owner">Owner</param>
		public void setOwner(Character owner) {
			this.owner = owner;
		}

		void Awake() {
			SetInitialReference();
		}

		//rotates renderer and counts down live remaining
		void Update() {
			if (--timeToLive < 0) {
				Debug.Log("Projectile died of old age");
				Destroy(this.gameObject);
			}
			cachedRenderer.transform.Rotate(Vector3.back * Time.deltaTime * velocity);
		}

		//destroy on collision
		public override void OnTriggerEnter2D(Collider2D col) {
			base.OnTriggerEnter2D(col);
			Destroy(this.gameObject);
		}
	}
}