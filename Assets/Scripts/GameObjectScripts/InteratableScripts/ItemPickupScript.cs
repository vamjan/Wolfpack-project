using UnityEngine;
using System.Collections;
using Wolfpack.Characters;

namespace Wolfpack {
	/// <summary>
	/// Script used on objects which can be picked up.
	/// </summary>
	public class ItemPickupScript : MonoBehaviour, IInteractable {

		public string item = null;
		public int count = 0;

		/// <summary>
		/// Specified player interacts with this object.
		/// Works with players inventory.
		/// Needed to implement IInteractable.
		/// </summary>
		/// <param name="player">Player</param>
		public void Interact(PlayerCharacterScript player) {
			player.UpdateInventory(item, (count > 0) ? count : 0);
			player.RemoveInteractable(this.gameObject);
			Destroy(this.gameObject);
		}
	}
}