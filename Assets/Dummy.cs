using UnityEngine;
using System.Collections;

public class Dummy : NPCharacter {

    public override void TakeDmg(float dmg)
    {
        base.TakeDmg(dmg);
        cachedAnim.SetTrigger("Hit");
    }
}
