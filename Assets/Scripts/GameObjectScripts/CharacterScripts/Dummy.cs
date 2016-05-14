using UnityEngine;
using System.Collections;

namespace Wolfpack.Characters
{
    /// <summary>
    /// Simple dummy script which triggers dummy animations when attacked
    /// </summary>
    public class Dummy : NPCharacter
    {

        public override void UpdateHealth(int dmg)
        {
            base.UpdateHealth(dmg);
            cachedAnim.SetTrigger("Hit");
        }
    }
}
