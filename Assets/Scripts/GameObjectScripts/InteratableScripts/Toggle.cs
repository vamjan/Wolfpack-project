using UnityEngine;
using System.Collections;
using Wolfpack.Characters;

namespace Wolfpack {
	public class Toggle : MonoBehaviour, IInteractable {

		private bool state = false;

		public void Interact(PlayerCharacter player) {
			this.Use();
		}

		private void Use() {
			ChangeState();
		}

		private void ChangeState() {
			state = !state;
		}
	}
}
