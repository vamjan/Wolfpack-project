using UnityEngine;
using System.Collections;

namespace Wolfpack {
	/// <summary>
	/// Trigger that reacts only to player entering.
	/// </summary>
	public class PlayerTriggerScript : TriggerScript {

		GameObject player = null;
		
		void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.Equals(player))
				OnEnter.Invoke(col);
		}

		void OnTriggerStay2D(Collider2D col)
		{
			if(col.gameObject.Equals(player))
				OnStay.Invoke(col);
		}

		void OnTriggerExit2D(Collider2D col)
		{
			if(col.gameObject.Equals(player))
				OnExit.Invoke(col);
		}

		void Awake() {
			player = GameObject.Find("Player");
		}
	}
}
