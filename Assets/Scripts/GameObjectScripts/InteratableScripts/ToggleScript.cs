using UnityEngine;
using System.Collections;
using Wolfpack.Characters;
using Wolfpack.Managers;

namespace Wolfpack {
	/// <summary>
	/// Toggle script for levers, doors etc.
	/// </summary>
	public class ToggleScript : MonoBehaviour, IInteractable {

		/// <summary>
		/// Specified player interacts with this object.
		/// Needed to implement IInteractable.
		/// </summary>
		/// <param name="player">Player</param>
		public void Interact(PlayerCharacterScript player) {
			this.Use();
		}

		/// <summary>
		/// Use this instance.
		/// Now is only used at the end of demo.
		/// Will be changed in near future.
		/// </summary>
		protected virtual void Use() {
			GameManager.instance.CallGameFinishedEvent();
		}
	}
}
