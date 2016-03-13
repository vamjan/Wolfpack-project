using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour, IMovable, IAttackable, IScriptable {
    public float damage;
    public float health;
    public bool isDeath;

    public float maxSpeed;
    public Vector2 heading;
    public Vector2 movement;

    //cached version of our physics rigid body.
    protected Rigidbody2D cachedRigidBody2D;

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

    public void Move(Vector2 velocity)
    {
        this.Turn(velocity);
        transform.Translate(movement);
        movement = Vector2.zero;
    }

    public void Turn(Vector2 heading)
    {
        this.heading = heading.normalized;
        Debug.Log(this.heading);
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	public virtual void Update () {
        Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(this.heading.x * 0.1f, this.heading.y * 0.1f, 0),
        Color.red, 0.1f, false);
    }
}
