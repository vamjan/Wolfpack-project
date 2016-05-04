using UnityEngine;
using System.Collections;
using System;

namespace Wolfpack.Character
{
    public class NPCharacter : Character, IKillable
    {
        public IEnumerator Die(int time)
        {
            cachedAnim.SetTrigger("Die");
            StartCoroutine(PauseMovement(time));
            yield return new WaitForSeconds(time);
        }
    }
}
