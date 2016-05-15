using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Wolfpack.Characters
{
    /// <summary>
    /// Script that controls the player input and controls the player character ingame.
    /// </summary>
    public class PlayerCharacterScript : Character, IControlable, IKillable
    {
		/// <summary>
		/// All those events are used to comunicate with Game and Player managers.
		/// GUI and gameloop updates.
		/// </summary>
        public delegate void HealthUpdate(int newHealth);
        public event HealthUpdate OnHealthUpdated;

		public delegate void ItemUpdate(string newItem, int count);
        public event ItemUpdate OnItemsUpdated;

        public delegate void Death();
        public event Death OnDeath;

		//list of interactables
        [SerializeField]
        private List<GameObject> interactables = new List<GameObject>();

		//combat flag
		public bool inCombat = true;

		/// <summary>
		/// Method called by triggers in game.
		/// Adds an object to interactable list.
		/// </summary>
		/// <param name="obj">Object</param>
        public void AddInteractable(GameObject obj)
        {
			inCombat = false;
            interactables.Add(obj);
			obj.GetComponent<SpriteRenderer>().material.color = Color.green;
        }

		/// <summary>
		/// Method called by triggers in game.
		/// Removes an object from interactable list.
		/// </summary>
		/// <param name="obj">Object</param>
		public void RemoveInteractable(GameObject obj) 
		{
			interactables.Remove(obj);
			obj.GetComponent<SpriteRenderer>().material.color = Color.white;
			if (interactables.Count == 0)
				inCombat = true;
		}

		/// <summary>
		/// Interact with the closest interatable in interatables list.
		/// </summary>
        public void Interact()
        {
			GameObject tmp;
			if ((tmp = FindClosest(interactables)) != null) {
				IInteractable obj = tmp.GetComponent<IInteractable>();
				if(obj != null)
				{
					obj.Interact(this);
				}
			} 
        }

		/// <summary>
		/// Finds the closest gameobject in given list.
		/// Is used to find closest target and interactable.
		/// Can have specified search radius (0 means it does not check for distance).
		/// </summary>
		/// <returns>The closest object</returns>
		/// <param name="objects">Objects</param>
		/// <param name="radius">Radius</param>
		public GameObject FindClosest(List<GameObject> objects, int radius = 0)
        {
            GameObject retval = null;
            float minDistance = float.MaxValue;

            foreach (var item in objects)
            {
				if (item != null) { //had some weird bugs, this here is neccesary AFAIK
					//TODO: check what is wrong with unity removing objects frm lists
					if (retval == null) {
						retval = item;
						minDistance = (this.transform.position - item.transform.position).magnitude;
					} else {
						float distance = (this.transform.position - item.transform.position).magnitude;
						if (distance < minDistance) {
							retval = item;
							minDistance = distance;
						}
					}
				}
            }

			if (radius == 0)
				return retval;
			else if (minDistance <= radius)
				return retval;
			else
				return null;
        }

		/// <summary>
		/// Die in the specified time.
		/// This has to be a coroutine to wait for death animation to play.
		/// </summary>
		/// <param name="time">Time</param>
        public IEnumerator Die(int time)
        {
            cachedAnim.SetTrigger("Die");
            StartCoroutine(PauseMovement(time));
            yield return new WaitForSeconds(time);
            if (OnDeath != null)
            {
                OnDeath();
            }
        }


		/// <summary>
		/// Sets the health to specified value.
		/// Used by player manager to set health at the start of level.
		/// </summary>
		/// <param name="value">Value</param>
        public void SetHealth(int value)
        {
            int change = value - health;
            UpdateHealth(change);
        }

		/// <summary>
		/// Method needed to implement IAttackable. Invokes health changes.
		/// Checks for dead and invokes death coroutine.
		/// Also updates GUI.
		/// </summary>
		/// <param name="value">Change value</param>
        public override void UpdateHealth(int value)
        {
            ChangeHealth(value);
            if (OnHealthUpdated != null)
            {
                OnHealthUpdated(health);
            }

            if (isDead)
            {
                StartCoroutine(Die(1));
            }
        }

		/// <summary>
		/// Updates the inventory and GUI.
		/// Used for picking up items.
		/// </summary>
		/// <param name="item">Item</param>
		/// <param name="count">Count</param>
		public void UpdateInventory(string item, int count)
        {
            if(OnItemsUpdated != null)
            {
                OnItemsUpdated(item, count);
            }
        }

		/// <summary>
		/// Locks the target.
		/// Character will now face the target.
		/// </summary>
        private void LockTarget()
        {
			Debug.Log("Lock " + target);
            turnLocked = true;
			this.direction = (target.transform.position - this.transform.position).normalized;

			targetScript = target.GetComponent<Character>();
			target.GetComponent<Renderer>().material.color = Color.red;
        }

		/// <summary>
		/// Oposite to locking. Releases target and starts free movement.
		/// </summary>
        private void ReleaseTarget()
        {
			Debug.Log("Release " + target);
			targetScript = null;
			target.GetComponent<Renderer>().material.color = Color.white;

            turnLocked = false;
			target = null;
        }

		/// <summary>
		/// Used to control user input.
		/// Changes targets and toggle lock/unlock if possible.
		/// </summary>
        public void ToggleTarget()
        {	
			//remember last target
			GameObject oldTarget = target;

			//throw away last target
			if (oldTarget != null)
				ReleaseTarget();

			//find new
			target = FindClosest(levelManager.Targetables, 200);

			//if new is not last targeted object, retarget to new
			//else release target altogether
			if (target != null) 
				if(!target.Equals(oldTarget)) {
					LockTarget();
				} else {
					ReleaseTarget();
				}
        }

		/// <summary>
		/// Needed only for demo to give player some advantage.
		/// Makes attack freeze shorter.
		/// </summary>
		public override void Attack()
		{
			float time = Animate("Attack");
			StartCoroutine(PauseMovement(time/2));
		}

		/// <summary>
		/// Coroutine to pause character when it is performing some action.
		/// </summary>
		/// <param name="time">Pause time</param>
		/// <returns>IEnumerator - necessary for coroutines</returns>
		protected override IEnumerator PauseMovement(float time)
		{
			cachedRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
			InputWrapper.inputEnabled = false;
			yield return new WaitForSeconds(time);
			cachedRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			InputWrapper.inputEnabled = true;
		}

		/// <summary>
		/// Update this instance each frame.
		/// </summary>
		public override void Update() 
		{
			base.Update();

			if (targetScript != null && targetScript.isDead) 
			{
				ReleaseTarget();
			}
		}
	}
}
