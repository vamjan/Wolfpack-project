using UnityEngine;
using System.Collections;
using Wolfpack.Characters;

namespace Wolfpack {
	/// <summary>
	/// Text board script. Used on every sign in the game.
	/// Creates dialogBox and manages it.
	/// </summary>
	public class SignScript : MonoBehaviour, IInteractable {

		//Message divided to shorter strings. Setup msg in Unity Editor.
		public string[] msg = null;
		//Prefab for dialogBox. Setup in Unity Editor.
		public GameObject dialogBoxPrefab = null;
		//Control script for dialogBox.
		private DialogBoxScript dialogBox = null;

		/// <summary>
		/// Specified player interacts with this object.
		/// Creates dialogBox and allows player to toggle through whole message.
		/// Needed to implement IInteractable.
		/// </summary>
		/// <param name="player">Player</param>
		public void Interact(PlayerCharacterScript player) {
			if (dialogBox == null)
				WriteMsg();
			else
				ToggleMessage();
		}

		/// <summary>
		/// Creates dialogBox and dialogBox controler script.
		/// Passes message to dialogBox.
		/// </summary>
		private void WriteMsg() {
			if(dialogBoxPrefab != null) {
				GameObject tmp = Instantiate(dialogBoxPrefab);
				dialogBox = tmp.GetComponent<DialogBoxScript>();
				dialogBox.textContent = msg;
			} else {
				Debug.Log(this + " DialogBox is missing!");
			}
		}

		/// <summary>
		/// Toggles the message.
		/// </summary>
		private void ToggleMessage() {
			dialogBox.ToggleTextContent();
		}
	}
}
