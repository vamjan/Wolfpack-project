using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour, IMovable, IAttackable, IScriptable {
    public float health;
    public bool isDead;
    public float maxSpeed;
    public float speed;
    public Vector2 direction;
    public Vector2 movement;
    public bool turnLocked;
    public bool idle;
    public GameObject target;

    //cached version of our physics rigid body.
    protected Collider2D attackHitbox;
    protected Collider2D ownHitbox;
    protected Rigidbody2D cachedRigidBody2D;
    protected SpriteRenderer rendererer;
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
        getIsometricVector(ref point);
        Move(point);

        anim.SetBool("Walking", !idle);
        anim.SetFloat("MovementX", direction.x);
        anim.SetFloat("MovementY", direction.y);

    }

    public void Move(Vector2 control)
    {
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
            if (!turnLocked) Turn(control);
            else Turn(this.target.transform.position - this.transform.position);
            setMoveAngle(control);
            accelerate(ref speed);
            movement = control * speed;
            cachedRigidBody2D.velocity = movement;
        }

        this.direction.Normalize();
    }

    public void Turn(Vector2 newHeading)
    {
        direction = newHeading;

        if (direction.x < 0)
        {
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
