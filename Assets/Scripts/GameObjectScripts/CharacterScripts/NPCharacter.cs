using UnityEngine;
using System.Collections;
using System;

namespace Wolfpack.Character
{
    public class NPCharacter : Character, IKillable
    {
        public void Die(int time)
        {
            cachedAnim.SetTrigger("Die");
            StartCoroutine(PauseMovement(time));
        }
    }
}
