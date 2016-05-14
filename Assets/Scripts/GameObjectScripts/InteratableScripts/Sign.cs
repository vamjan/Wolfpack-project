using UnityEngine;
using System.Collections;
using Wolfpack.Characters;

namespace Wolfpack {
	public class Sign : MonoBehaviour, IInteractable {

		public string[] msg = null;
		public GameObject dialogBoxPrefab = null;
		private DialogBoxScript dialogBox = null;

		public void Interact(PlayerCharacter player) {
			if (dialogBox == null)
				WriteMsg();
			else
				ToggleMessage();
		}

		private void WriteMsg() {
			if(dialogBoxPrefab != null) {
				GameObject tmp = Instantiate(dialogBoxPrefab);
				dialogBox = tmp.GetComponent<DialogBoxScript>();
				dialogBox.textContent = msg;
			} else {
				Debug.Log(this + " DialogBox is missing!");
			}
		}

		private void ToggleMessage() {
			dialogBox.ToggleTextContent();
		}
	}
}
