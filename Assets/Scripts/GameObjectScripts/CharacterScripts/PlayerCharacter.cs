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

        public void Die(int time)
        {
            throw new NotImplementedException();
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
