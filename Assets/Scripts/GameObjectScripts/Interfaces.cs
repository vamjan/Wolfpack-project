using UnityEngine;
using System.Collections;

namespace Wolfpack
{
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
        void TakeDmg(float dmg);
    }

    public interface IKillable
    {
        void Die(int time);
    }

    public interface ISparable
    {
        void Spare();
    }

    public interface ITalkable
    {
        void Talk(string text);
    }

    public interface ITakable
    {
        void Take();
    }
}