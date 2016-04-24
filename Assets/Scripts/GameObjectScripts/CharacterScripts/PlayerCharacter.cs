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

        public void Die(int time)
        {
            throw new NotImplementedException();
        }

        public void SetHealth(int value)
        {
            int change = value - health;
            ChangeHealth(change);
            if (OnHealthUpdated != null)
            {
                OnHealthUpdated(health);
            }
        }

        public override void UpdateHealth(int value)
        {
            base.UpdateHealth(value);
            if (OnHealthUpdated != null)
            {
                OnHealthUpdated(health);
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

        // Update is called once per frame
        public override void Update()
        {

            base.Update();

            // Gets information about input axis
            float inputX = InputWrapper.GetAxisRaw("Horizontal");
            float inputY = InputWrapper.GetAxisRaw("Vertical");


            // Prepare the movement for each direction
            this.movement = new Vector2(inputX, inputY);

            // Makes the movement relative to time
            movement *= Time.deltaTime;

            Walk(movement.normalized);

            //get targeting input from player
            bool target = InputWrapper.GetButtonDown("Target");

            if (target)
            {
                ToggleTarget();
            }

            //get attack input from player
            bool atk = InputWrapper.GetButtonDown("Attack light");

            if (atk)
            {
                Attack();
            }
        }
    }
}
