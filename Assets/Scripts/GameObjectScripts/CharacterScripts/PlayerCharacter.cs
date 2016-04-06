using UnityEngine;
using System.Collections;
using System;

public class PlayerCharacter : Character, IControlable, IKillable {

    public void Die(int time)
    {
        throw new NotImplementedException();
    }

    private void lockTarget()
    {
        turnLocked = true;
        this.direction = target.transform.position - this.transform.position;
    }

    private void releaseTarget()
    {
        turnLocked = false;
    }

    public void toggleTarget()
    {
        if (turnLocked) releaseTarget();
        else lockTarget();
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
            attack();
        }
    }
}
