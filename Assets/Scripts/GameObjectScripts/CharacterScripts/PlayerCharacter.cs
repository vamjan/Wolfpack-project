using UnityEngine;
using System.Collections;
using System;

namespace Wolfpack.Character
{
    /// <summary>
    /// Script that controls the player input and controls the player character ingame.
    /// </summary>
    public class PlayerCharacter : Character, IControlable, IKillable
    {

        public delegate void HealthUpdate(int newHealth);
        public event HealthUpdate OnHealthUpdated;

        public delegate void ItemUpdate(string newItem);
        public event ItemUpdate OnItemsUpdated;

        public delegate void Death();
        public event Death OnDeath;

        public void Die(int time)
        {
            cachedAnim.SetTrigger("Die");
            StartCoroutine(PauseMovement(time));
            if (OnDeath != null)
            {
                OnDeath();
            }
        }

        public void SetHealth(int value)
        {
            int change = value - health;
            ChangeHealth(change);
            if (OnHealthUpdated != null)
            {
                OnHealthUpdated(health);
            }

            if(isDead)
            {
                Die(1);
            }
        }

        public override void UpdateHealth(int value)
        {
            base.UpdateHealth(value);
            if (OnHealthUpdated != null)
            {
                OnHealthUpdated(health);
            }

            if (isDead)
            {
                Die(1);
            }
        }

        public void updateInventory(string item)
        {
            if(OnItemsUpdated != null)
            {
                OnItemsUpdated(item);
            }
        }

        // Lock target to lock direction
        private void LockTarget()
        {
            turnLocked = true;
            this.direction = target.transform.position - this.transform.position;
        }

        // Unlock target
        private void ReleaseTarget()
        {
            turnLocked = false;
        }

        // Used for user input to switch between target locking and free mode
        public void ToggleTarget()
        {
            if (turnLocked) ReleaseTarget();
            else LockTarget();
        }
    }
}
