using UnityEngine;
using System.Collections;
using System;
using Wolfpack.Managers;
using System.ComponentModel;
using System.Linq;

namespace Wolfpack.Characters
{
    /// <summary>
    /// Base character script. Contains all generic functions and variables which all game characters might need.
    /// </summary>
    public class Character : MonoBehaviour, IMovable, IAttackable, IScriptable
    {
        public int maxHealth;
        //character maximum hit points
        public int health;
        //when hit points reach zero, character dies
        public float maxSpeed;
		//current runing speed (not the same as max speed most of the times)
        protected float speed = 0;
        //direction of movement, normalized vector
        public Vector2 direction;
        //actual movement vector
        public Vector2 movement;
        //determines if the character can turn by movement or not
		public bool isDead = false;
		//maximum runing speed (set by editor)
		protected bool turnLocked = false;
        //determines if character is moving or is idle
		protected bool idle = true;
		//determines if character is in dodging motion
		protected bool dodging = false;
		//time needed to perform dodge motion (defaulted to 0.5 sec)
		protected float dodgeTime = 0.5f;
		//maximum amout of character poise
		protected const int MAX_POISE = 20;
		//amount of damage before character gets staggered
		protected int poise = MAX_POISE;
		//enum alegiance, to avoid damaging allied characters
		public Alegiance al;
        //targeted enemy
		[SerializeField]
        protected GameObject target;

		protected Character targetScript;
        //cached version of important components (for performance reasons)
        //hitbox used for attacking
        protected Collider2D cachedAttackHitbox;
        //hitbox used for collision detection with enviroment and other attack hitboxes
        protected Collider2D cachedOwnHitbox;
        //rigid body used for movement in Unity
        protected Rigidbody2D cachedRigidBody2D;
        //character sprite renderer
        protected SpriteRenderer cachedRenderer;
        //character animator (contains all animations)
        protected Animator cachedAnim;
		//level manager reference for targeting
        protected LevelManager levelManager;

		/// <summary>
		/// Attack invoked by another script (manager).
		/// </summary>
        public virtual void DoScriptAttack()
        {
			Attack();
        }

		/// <summary>
		/// Death invoked by another script (manager).
		/// Does not take health in consideration.
		/// </summary>
		/// <param name="time">Time</param>
		public virtual void DoScriptDie(int time)
        {
			throw new NotImplementedException();
        }

		/// <summary>
		/// Movement invoked by another script (manager).
		/// Moves towards target destination normaly.
		/// </summary>
		/// <param name="destination">Destination</param>
        public virtual void DoScriptMove(Vector2 destination)
        {
			Walk(destination.normalized);
        }

		/// <summary>
		/// Method needed to implement IAttackable. Invokes health changes.
		/// </summary>
		/// <param name="value">Change value</param>
        public virtual void UpdateHealth(int value)
        {
            ChangeHealth(value);
        }

		/// <summary>
		/// Changes health by given amount.
		/// If character drops below 0 health, it flags him as dead.
		/// If character takes enough damage to get staggered, it will get staggered.
		/// </summary>
		/// <param name="value">Change value</param>
		protected void ChangeHealth(int value)
        {
            health += value;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
            if (health <= 0)
            {
                isDead = true;
			} else if (value < 0) {
				poise += value;
				if(poise <= 0) {
					Stagger();
					poise = MAX_POISE;
				}
			}
        }

		/// <summary>
		/// Character drinks the potion. Only health potions are implemented so far.
		/// Second parameter is defaulted to 0 as healing potion.
		/// </summary>
		/// <param name="health">Health</param>
		/// <param name="effect">Effect - default 0 as healing</param>
		public void DrinkPotion(int health, int effect = 0) {
			//only healing potions implemented so far
			float time = Animate("Drink");
			StartCoroutine(PauseMovement(time/3*2));
			UpdateHealth(health);
			StartCoroutine(PauseMovement(time/3));
		}

		/// <summary>
		/// Walk to specified point.
		/// Wraps all methods needed to walk.
		/// </summary>
		/// <param name="point">Point</param>
        public void Walk(Vector2 point)
        {
            GetIsometricVector(ref point);              //get isometric vector instead of 45 degree variations
            Move(point);                                //move character

            //animation control (sending parameters to animator)
            cachedAnim.SetBool("Walking", !idle);
            cachedAnim.SetFloat("MovementX", direction.x);
            cachedAnim.SetFloat("MovementY", direction.y);

        }

        /// <summary>
        /// Execute movement towards control vector point. Uses rigidbody2D and Unity build in physics system.
		/// Needed to implement IMovable.
        /// </summary>
        /// <param name="control">Control vector for movement direction</param>
        public virtual void Move(Vector2 control)
        {
            //don't move when control vector is zero
            if (control.x == 0 && control.y == 0)
            {
                idle = true;
                speed = 0;
                movement = Vector2.zero;
            }
            else
            {
                idle = false;
                //turn with movement of not turn locked
                if (!turnLocked) Turn(control);
                //get angle of movement to movement direction fo animator
                SetMoveAngle(control);
				if (dodging)
					Dodge(ref speed);
				else
                	Accelerate(ref speed);
                movement = control * speed;
                cachedRenderer.sortingOrder = levelManager.GetLayer(transform.position.y);
            }
            cachedRigidBody2D.velocity = movement;
        }

		/// <summary>
		/// Turn the towards newHeading.
		/// Needed to implement IMovable.
		/// </summary>
		/// <param name="newHeading">New heading</param>
        public virtual void Turn(Vector2 newHeading)
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

			direction.Normalize();
        }

        /// <summary>
        /// When character is target locked. The movement angle is diferent from direction angle. This method calculates this angle.
        /// </summary>
        /// <param name="newHeading">Movement heading</param>
        private void SetMoveAngle(Vector2 newHeading)
        {
            float movementAngle = Vector2.Angle(direction, newHeading);
            cachedAnim.SetFloat("MovementAngle", movementAngle);
        }

        /// <summary>
        /// Changes input vectors to isometric ones. Meaning - 45 degrees => 30 degrees etc.
        /// </summary>
        /// <param name="control"></param>
        private void GetIsometricVector(ref Vector2 control)
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

		/// <summary>
		/// Accelerate the specified speed.
		/// </summary>
		/// <param name="speed">referenced Speed</param>
        private void Accelerate(ref float speed)
        {
            if (speed < maxSpeed)
            {
				speed += (maxSpeed - speed) / (0.15f/Time.deltaTime);
                speed = Mathf.Ceil(speed);
			} else {
				speed = maxSpeed;
			}
        }

		/// <summary>
		/// Sets dodge flag to true. Character will switch to dodge motion for next dodgeTime value.
		/// </summary>
		public void StartDodge() {
			if(movement.normalized != Vector2.zero)
				dodging = true;
		}

		/// <summary>
		/// Counterpart to Accelerate method, but for dodge motion.
		/// </summary>
		/// <param name="speed">referenced Speed</param>
		private void Dodge(ref float speed) {
			speed = maxSpeed * 2;
			dodgeTime -= Time.deltaTime * 2;
			if (dodgeTime < 0) {
				dodging = false;
				dodgeTime = 0.5f;
			}
		}

		/// <summary>
		/// Shoot the specified projectilePrefab, damage, velocity and timeToLive.
		/// Wraps all neccesary methods to shoot a projectile.
		/// </summary>
		/// <param name="projectilePrefab">Projectile prefab</param>
		/// <param name="damage">Damage</param>
		/// <param name="velocity">Velocity</param>
		/// <param name="timeToLive">Time to live</param>
		public virtual void Shoot(GameObject projectilePrefab, int damage, float velocity, int timeToLive) {
			//Vector3 tmpTarget = (target == null) ? (Vector3)direction : target.transform.position.normalized;
			Vector3 tmpTarget = (Vector3)direction;
			float time = Animate("Throw");
			StartCoroutine(PauseMovement(time/2)); //until end of animation
			CreateProjectile(projectilePrefab, damage, velocity, timeToLive, tmpTarget);
			StartCoroutine(PauseMovement(time/2)); //backswing
		}

		/// <summary>
		/// Creates the projectile from specified prefab.
		/// Initilazes control script and adds it to projectile.
		/// </summary>
		/// <param name="projectilePrefab">Projectile prefab</param>
		/// <param name="damage">Damage</param>
		/// <param name="velocity">Velocity</param>
		/// <param name="timeToLive">Time to live</param>
		/// <param name="projectileTarget">Projectile target</param>
		public void CreateProjectile(GameObject projectilePrefab, int damage, float velocity, int timeToLive, Vector3 projectileTarget) {
			GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position + (Vector3)direction * 20, transform.rotation);
			//unity does not like NEW, so I have to initialize controler script like this :(

			ProjectileScript projectilController = projectile.AddComponent<ProjectileScript>();
			projectilController.velocity = velocity;
			projectilController.damage = damage;
			projectilController.timeToLive = timeToLive;
			projectilController.setOwner(this);

			projectile.GetComponent<Rigidbody2D>().velocity = projectileTarget * velocity;
		}

		/// <summary>
		/// Method to invoke attacking by control script.
		/// </summary>
        public virtual void Attack()
        {
			float time = Animate("Attack");
			StartCoroutine(PauseMovement(time));
        }

		/// <summary>
		/// Stagger when you take enough damage.
		/// </summary>
		public virtual void Stagger() {
			float time = Animate("Stagger");
			StartCoroutine(PauseMovement(time));
		}

		/// <summary>
		/// Sets animation trigger in cachedAnim.
		/// Then gets animation clip lenght in seconds and returns it.
		/// </summary>
		/// <param name="anim">Animation</param>
		protected float Animate(string anim) {
			cachedAnim.SetTrigger(anim);
			AnimationClip c = cachedAnim.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name.Equals("Mainchar_" + anim.ToLower() + "_side"));
			if (c != null) 
				return c.length;
			else
				return 1.0f;
		}

        /// <summary>
        /// Coroutine to pause character when it is performing some action.
        /// </summary>
        /// <param name="time">Pause time</param>
        /// <returns>IEnumerator - necessary for coroutines</returns>
        protected virtual IEnumerator PauseMovement(float time)
        {
            cachedRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(time);
            cachedRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
       
        // Use this for initialization
        void Awake()
        {
            cachedAnim = GetComponent<Animator>();
            cachedRigidBody2D = GetComponent<Rigidbody2D>();
            cachedRenderer = GetComponent<SpriteRenderer>();
            cachedOwnHitbox = GetComponent<Collider2D>();
            cachedAttackHitbox = GetComponentInChildren<Collider2D>();

            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }

        // Update is called once per frame
        public virtual void Update()
        {
			Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x * 20, direction.y * 20, 0),
				Color.red, 0.0f, false);

			//keep turning with target
			if(target != null) {
				Turn(target.transform.position - transform.position);
			}
        }
    }
}
