using UnityEngine;

using System.Collections;
using System;

public class PlayerControl : MonoBehaviour

{
	//max speed of character
	public float maxSpeed =5.0f;

	public KeyCode moveUp;
	public KeyCode moveDown;
	public KeyCode moveRight;
	public KeyCode moveLeft;

	//cached version of our physics rigid body.
	private Rigidbody2D cachedRigidBody2D;

	void Awake()
	{
	}

	private void Start()
	{
		this.cachedRigidBody2D = this.GetComponent<Rigidbody2D>();
	}

	public void Move(Vector2 movement)
	{
		//move the rigid body, which is part of the physics system
		//This ensures smooth movement.
		this.cachedRigidBody2D.velocity = new Vector2(movement.x * maxSpeed, movement.y * maxSpeed);
	}

}
