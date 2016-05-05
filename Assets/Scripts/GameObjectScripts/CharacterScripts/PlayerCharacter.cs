using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Wolfpack.Character
{
    /// <summary>
    /// Script that controls the player input and controls the player character ingame.
    /// </summary>
    public class PlayerCharacter : Character, IControlable, IKillable
    {

        public delegate void HealthUpdate(int newHealth);
        public event HealthUpdate OnHealthUpdated;

        public delegate void ItemUpdate(string newItem);
        public event ItemUpdate OnItemsUpdated;

        public delegate void Death();
        public event Death OnDeath;

        [SerializeField]
        private List<GameObject> interactables = new List<GameObject>();

        public void AddInteratable(GameObject obj)
        {
            /*if ((tmp = col.gameObject.GetComponent(typeof(IAttackable)) as Component) != null)
            {
                (tmp as IAttackable).UpdateHealth(damage);
            }

            if ()*/
            interactables.Add(obj);
        }

        public void Interact()
        {
            IInteractable obj = FindClosest(interactables).GetComponent<IInteractable>();
            if(obj != null)
            {
                obj.Interact();
            }
        }

        public GameObject FindClosest(List<GameObject> objects)
        {
            GameObject retval = null;
            float minDistance = float.MaxValue;

            foreach (var item in objects)
            {
                if (retval == null)
                {
                    retval = item;
                    minDistance = (this.transform.position - item.transform.position).magnitude;
                }
                else
                {
                    float distance = (this.transform.position - item.transform.position).magnitude;
                    if (distance < minDistance)
                    {
                        retval = item;
                        minDistance = distance;
                    }
                }
            }
            return retval;
        }

        public IEnumerator Die(int time)
        {
            cachedAnim.SetTrigger("Die");
            StartCoroutine(PauseMovement(time));
            yield return new WaitForSeconds(time);
            if (OnDeath != null)
            {
                OnDeath();
            }
        }

        public void SetHealth(int value)
        {
            int change = value - health;
            UpdateHealth(change);
        }

        public override void UpdateHealth(int value)
        {
            ChangeHealth(value);
            if (OnHealthUpdated != null)
            {
                OnHealthUpdated(health);
            }

            if (isDead)
            {
                StartCoroutine(Die(1));
            }
        }

        public void updateInventory(string item)
        {
            if(OnItemsUpdated != null)
            {
                OnItemsUpdated(item);
            }
        }

        // Lock target to lock direction
        private void LockTarget()
        {
            turnLocked = true;
            this.direction = target.transform.position - this.transform.position;
        }

        // Unlock target
        private void ReleaseTarget()
        {
            turnLocked = false;
        }

        // Used for user input to switch between target locking and free mode
        public void ToggleTarget()
        {
            if (turnLocked) ReleaseTarget();
            else LockTarget();
        }
    }
}
