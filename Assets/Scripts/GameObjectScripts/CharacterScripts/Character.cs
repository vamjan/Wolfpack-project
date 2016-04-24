using UnityEngine;
using System.Collections;
using System;
using Wolfpack.Managers;
using System.ComponentModel;

namespace Wolfpack.Character
{
    /// <summary>
    /// Base character script. Contains all generic functions and variables which all game characters might need.
    /// </summary>
    public class Character : MonoBehaviour, IMovable, IAttackable, IScriptable
    {
        public int maxHealth;
        //character hit points
        [SerializeField]
        protected int health;
        //when hit points reach zero, character dies
        [SerializeField]
        protected bool isDead;
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
        protected Collider2D cachedAttackHitbox;
        //hitbox used for collision detection with enviroment and other attack hitboxes
        protected Collider2D cachedOwnHitbox;
        //rigid body used for movement in Unity
        protected Rigidbody2D cachedRigidBody2D;
        //character sprite renderer
        protected SpriteRenderer cachedRenderer;
        //character animator (contains all animations)
        protected Animator cachedAnim;

        protected LevelManager levelManager;


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

        public virtual void UpdateHealth(int value)
        {
            ChangeHealth(value);
        }

        protected void ChangeHealth(int value)
        {
            if (health < maxHealth)
            {
                health += value;
                if(health > maxHealth)
                {
                    health = maxHealth;
                }
            }
            if (health <= 0)
            {
                isDead = true;
            }
        }

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
        /// 
        /// 
        /// </summary>
        /// <param name="control">Control vector for movement direction</param>
        public void Move(Vector2 control)
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
                //keep tracking target when turn locked
                else Turn(target.transform.position - transform.position);
                //get angle of movement to movement direction fo animator
                SetMoveAngle(control);
                Accelerate(ref speed);
                movement = control * speed;
                cachedRenderer.sortingOrder = levelManager.GetLayer(transform.position.y);
            }
            cachedRigidBody2D.velocity = movement;
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

        private void Accelerate(ref float speed)
        {
            if (speed < maxSpeed)
            {
                speed += (maxSpeed - speed) / 5;
                speed = Mathf.Ceil(speed);
            }
        }

        protected void Attack()
        {
            cachedAnim.SetTrigger("Attack");
            StartCoroutine(PauseMovement(0.5f));
        }

        /// <summary>
        /// Coroutine to pause character when it is performing some action.
        /// </summary>
        /// <param name="time">Pause time</param>
        /// <returns>IEnumerator - necessary for coroutines</returns>
        private IEnumerator PauseMovement(float time)
        {
            cachedRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(time);
            cachedRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Use this for initialization
        void Start()
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
        }
    }
}
