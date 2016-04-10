using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour, IMovable, IAttackable, IScriptable {
    //character hit points
    public float health;
    //when hit points reach zero, character dies
    public bool isDead;
    //maximum runing speed
    public float maxSpeed;
    //current runing speed (not the same as max speed most of the times
    public float speed;
    //direction of movement, normalized vector
    public Vector2 direction;
    //actual movement vector
    public Vector2 movement;
    //determines if the character can turn by movement or not
    public bool turnLocked;
    //determines if character is moving or is idle
    public bool idle;
    //targeted enemy
    public GameObject target;

    //cached version of important components (for performance reasons)
    //hitbox used for attacking
    protected Collider2D attackHitbox;
    //hitbox used for collision detection with enviroment and other attack hitboxes
    protected Collider2D ownHitbox;
    //rigid body used for movement in Unity
    protected Rigidbody2D cachedRigidBody2D;
    //character sprite renderer
    protected SpriteRenderer rendererer;
    //character animator (contains all animations)
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
        health -= dmg;
        if (health <= 0)
        {
            isDead = true;
        }
    }

    public void Walk(Vector2 point)
    {
        getIsometricVector(ref point);              //get isometric vector instead of 45 degree variations
        Move(point);                                //move character

        //animation control (sending parameters to animator)
        anim.SetBool("Walking", !idle);
        anim.SetFloat("MovementX", direction.x);
        anim.SetFloat("MovementY", direction.y);

    }

    public void Move(Vector2 control)
    {
        //don't move when control vector is zero
        if (control.x == 0 && control.y == 0)
        {
            idle = true;
            speed = 0;
            movement = Vector2.zero;
            cachedRigidBody2D.velocity = movement;
        }
        else
        {
            idle = false;
            //turn with movement of not turn locked
            if (!turnLocked) Turn(control);
            //keep tracking target when turn locked
            else Turn(target.transform.position - transform.position);
            //get angle of movement to movement direction
            setMoveAngle(control);
            accelerate(ref speed);
            movement = control * speed;
            cachedRigidBody2D.velocity = movement;
        }

        direction.Normalize();
    }

    public void Turn(Vector2 newHeading)
    {
        direction = newHeading;

        //turning sprite renderer to simulate opposite sprites
        if (direction.x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void setMoveAngle(Vector2 newHeading)
    {
        float movementAngle = Vector2.Angle(direction, newHeading);
        anim.SetFloat("MovementAngle", movementAngle);
    }

    private void getIsometricVector(ref Vector2 control)
    {
        if (Math.Abs(control.x) != 1.0f)
        {
            control.x = (float)Math.Round(control.x) * 0.866f;
        }

        if (Math.Abs(control.y) != 1.0f)
        {
            control.y = (float)Math.Round(control.y) * 0.5f;
        }
    }

    private void accelerate(ref float speed)
    {
        if (speed < maxSpeed)
        {
            speed += (maxSpeed - speed) / 5;
            speed = Mathf.Ceil(speed);
        }
    }

    public void attack()
    {
        anim.SetTrigger("Attack");
        StartCoroutine(pauseMovement(0.5f));
    }

    private IEnumerator pauseMovement(float time)
    {
        cachedRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(time);
        cachedRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        cachedRigidBody2D = GetComponent<Rigidbody2D>();
        rendererer = GetComponent<SpriteRenderer>();
        ownHitbox = GetComponent<Collider2D>();
        attackHitbox = GetComponentInChildren<Collider2D>();
    }
	
	// Update is called once per frame
	public virtual void Update () {
        Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x * 20, direction.y * 20, 0),
        Color.red, 0.0f, false);
    }
}
