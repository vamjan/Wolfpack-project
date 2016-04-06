using UnityEngine;
using System.Collections;
using System;

public class PlayerCharacter : Character, IControlable, IKillable {

    public void Die(int time)
    {
        throw new NotImplementedException();
    }

    public void toggleFreeze()
    {
        InputWrapper.inputEnabled = !InputWrapper.inputEnabled;
    }

    // Update is called once per frame
    public override void Update() {

        base.Update();

        // Gets information about input axis
        float inputX = InputWrapper.GetAxisRaw("Horizontal");
        float inputY = InputWrapper.GetAxisRaw("Vertical");


        // Prepare the movement for each direction
        this.movement = new Vector2(inputX, inputY);

        // Makes the movement relative to time
        movement *= Time.deltaTime;

        Walk(movement.normalized);

        bool target = InputWrapper.GetButtonDown("Target");

        if(target)
        {
            toggleTarget();
        }

        bool atk = InputWrapper.GetButtonDown("Attack light");

        if(atk)
        {
            Debug.Log("Attack");
            attack();
        }
    }
}
