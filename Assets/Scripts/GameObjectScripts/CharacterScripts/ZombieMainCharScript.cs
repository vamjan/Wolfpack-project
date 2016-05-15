using UnityEngine;
using System.Collections;

namespace Wolfpack.Characters {
	public class ZombieMainCharScript : NPCharacter {

		//distance from player
		private float distance = 0;
		//attack cooldown, counts down
		private float attackCooldown = 1.0f;

		// Use this for initialization
		void Start () {
			this.target = levelManager.player.gameObject;
		}
		
		// Update is called once per frame
		public override void Update () {
			base.Update();

			//realy basic "follow and attack" script for demo
			if (!isDead) {
				distance = (target.transform.position - this.transform.position).magnitude;

				attackCooldown += Time.deltaTime/2;

				if (distance > 20)
					DoScriptMove((target.transform.position - this.transform.position).normalized);
				else {
					DoScriptMove(Vector2.zero);
					if(attackCooldown >= 1.0f) {
						DoScriptAttack();
						attackCooldown = 0f;
					}
				}
			}
				
			if (this.cachedRenderer.material.color != Color.red)
				this.cachedRenderer.material.color = Color.green;
		}
	}
}
