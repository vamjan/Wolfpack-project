using UnityEngine;
using System.Collections;

namespace Wolfpack {
	public class Projectile : AttackHitboxScript {

		public float velocity;
		public int timeToLive;

		private SpriteRenderer cachedRenderer;

		private void SetInitialReference() {
			cachedRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		void Awake() {
			SetInitialReference();
		}

		void Update() {
			if (--timeToLive < 0) {
				Debug.Log("Projectile died of old age");
				Destroy(this.gameObject);
			}
			cachedRenderer.transform.Rotate(Vector3.back * Time.deltaTime * velocity);
		}

		public override void OnTriggerEnter2D(Collider2D col) {
			base.OnTriggerEnter2D(col);
			Destroy(this.gameObject);
		}
	}
}