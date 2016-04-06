using UnityEngine;
using System.Collections;
using System;

public class PlayerCharacter : Character, IControlable, IKillable {



    public void Die(int time)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    public override void Update() {

        base.Update();

        // Gets information about input axis
        float inputX = InputWrapper.GetAxisRaw("Horizontal");
        float inputY = InputWrapper.GetAxisRaw("Vertical");

        /*if (inputX == 0.0f && inputY == 0.0f)
        {
            if (!idle)
            {
                idle = true;
                anim.SetBool("Walking", idle);
                Debug.Log(idle);
            }
        }
        else
        {*/
            // Prepare the movement for each direction
            this.movement = new Vector2(inputX, inputY).normalized;
            // Makes the movement relative to time

            movement *= Time.deltaTime;

            Walk(movement);
        //}
    }
}
