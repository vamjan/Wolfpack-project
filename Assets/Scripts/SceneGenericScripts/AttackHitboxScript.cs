using UnityEngine;
using System.Collections;

namespace Wolfpack
{
    public class AttackHitboxScript : MonoBehaviour
    {

        public int damage;

        void OnTriggerEnter2D(Collider2D col)
        {
            Component tmp;
            Debug.Log("Collider hit something! " + col.ToString());
            if ((tmp = col.gameObject.GetComponent(typeof(IAttackable)) as Component) != null)
            {
                (tmp as IAttackable).UpdateHealth(damage);
            }
        }
    }
}
