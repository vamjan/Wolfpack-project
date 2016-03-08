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
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX == 0.0 && inputY == 0.0)
        {

        }
        else
        {
            // Prepare the movement for each direction
            this.movement = new Vector2(
                maxSpeed * inputX,
                maxSpeed * inputY);
            // Makes the movement relative to time
            movement *= Time.deltaTime;

            Move(movement);
        }
    }
}
