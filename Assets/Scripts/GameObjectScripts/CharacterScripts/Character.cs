using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour, IMovable, IAttackable, IScriptable {
    public float damage;
    public float health;
    public bool isDead;

    public float maxSpeed;
    public float speed;
    public Vector2 heading;
    public Vector2 movement;
    public bool turnLocked;
    public bool idle;

    //cached version of our physics rigid body.
    protected Rigidbody2D cachedRigidBody2D;
    protected Animator anim;

    public virtual void DoScriptAttack()
    {
        throw new NotImplementedException();
    }

    public virtual void DoScriptDie()
    {
        throw new NotImplementedException();
    }

    public virtual void DoScriptMove(int x, int y)
    {
        throw new NotImplementedException();
    }

    public virtual void TakeDmg(float dmg)
    {
        throw new NotImplementedException();
    }

    public void Walk(Vector2 point)
    {
        Move(point);
    }

    public void Move(Vector2 velocity)
    {
        if (velocity.x == 0 && velocity.y == 0)
        {
            idle = false;
            anim.SetBool("Walking", idle);
            this.movement = Vector2.zero;
            cachedRigidBody2D.velocity = movement;
        }
        else
        {
            idle = true;
            anim.SetBool("Walking", idle);
            if (!turnLocked) Turn(velocity);
            this.movement = velocity * maxSpeed;
            cachedRigidBody2D.velocity = movement;
        }
    }

    public void Turn(Vector2 newHeading)
    {
        float movementAngle = Vector2.Angle(this.heading, newHeading);
        this.heading = newHeading.normalized;

        Debug.Log(movementAngle);

        anim.SetFloat("MovementX", this.heading.x);
        anim.SetFloat("MovementY", this.heading.y);
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        cachedRigidBody2D = this.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	public virtual void Update () {
        Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(this.heading.x * 20, this.heading.y * 20, 0),
        Color.red, 0.1f, false);
    }
}
