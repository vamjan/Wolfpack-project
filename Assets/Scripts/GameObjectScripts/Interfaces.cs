using UnityEngine;
using System.Collections;

namespace Wolfpack
{
    public enum ActionType { TAKE, USE, INTERACT }

    public enum Target { SELF, ENEMY };

    public interface IScriptable
    {
        void DoScriptMove(int x, int y);
        void DoScriptAttack();
        void DoScriptDie();
    }

    public interface IControlable
    {

    }

    public interface IMovable
    {
        void Move(Vector2 velocity);
        void Turn(Vector2 heading);
    }

    public interface IAttackable
    {
        void UpdateHealth(int dmg);
    }

    public interface IKillable
    {
        IEnumerator Die(int time);
    }

    public interface IInteractable
    {
        void Interact();
    }
}