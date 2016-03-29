using UnityEngine;

using System.Collections;

//require Character Script to be attached to this object.
[RequireComponent(typeof(PlayerCharacter))]
public class ControlsInput : MonoBehaviour {
	//private reference to the character script for making calls to the public api.
	private PlayerCharacter character;
	//reference to the camera
	private Camera mainCamera;

	private Vector2 heading;

	/// <summary>
	/// Use this function for initialization of just this component.
	/// </summary>
	private void Awake () 
	{
		//nothing special to initialize here.
		heading = Vector2.zero;
	}

	/// <summary>
	/// Use this function for initialization that depends on other components being created.
	/// </summary>
	private void Start()
	{
		//we require a built up version of the character script.
		this.character = this.GetComponent<PlayerCharacter>();

		this.mainCamera = Camera.main;
	}

	/// <summary>
	/// use this function to process updates as fast as the game can process them.
	/// </summary>
	void Update()
	{

        if (Input.GetKey(KeyCode.P))
        {
            character.GetComponent<Rigidbody2D>().freezeRotation = false;
        }
		
	}

	/// <summary>
	/// use this function to process updates that should be synchronized 
	/// with the physics engine.  Good for continuous input functions for movement.
	/// </summary>
	void FixedUpdate()
	{
		//get the x factor of movement.
		float xMovement = Input.GetAxisRaw("Horizontal");
		//get the y factor of movement.
		float yMovement = Input.GetAxisRaw("Vertical");

		Vector2 movement = new Vector2(xMovement, yMovement);

		if (movement.magnitude > 0)
		{
			this.heading = movement.normalized;
		}

		Vector3 worldPosition = this.mainCamera.ScreenToWorldPoint(new Vector3(this.heading.x, this.heading.y, 15));

		//use our character script reference to make a call into its public api
		//to move the character by our input factor.
		character.Move(heading);
	}
}