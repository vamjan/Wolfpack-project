using UnityEngine;
using System.Collections;
using Wolfpack.Characters;

namespace Wolfpack {
	public class Item : MonoBehaviour, IInteractable {

		//TODO: item enumeration
		public string item = null;
		public int count = 0;

		public void Interact(PlayerCharacter player) {
			player.UpdateInventory(item, (count > 0) ? count : 0);
			player.RemoveInteractable(this.gameObject);
			Destroy(this.gameObject);
		}
	}
}