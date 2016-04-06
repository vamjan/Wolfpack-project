using UnityEngine;
using System.Collections;

public class AttackHitboxScript : MonoBehaviour {

    public float damage;

    void OnTriggerEnter2D(Collider2D col)
    {
        Component tmp;
        Debug.Log("Collider hit something! " + col.ToString());
        if((tmp = col.gameObject.GetComponent(typeof(IAttackable)) as Component) != null)
        {
            (tmp as IAttackable).TakeDmg(damage);
        }
    }
}
