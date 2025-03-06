using System;
using UnityEngine;

public interface IHittable
{
    void hit(Attack attack);
}

[Serializable]
public struct Attack
{
    public int damage;
    public int stun;
    public int revenge;
    public bool isSpecial;
    public int attackID;

    public Attack(int damage, int stun, int revenge, bool isSpecial, int attackID)
    {
        this.damage = damage;
        this.stun = stun;
        this.revenge = revenge;
        this.isSpecial = isSpecial;
        this.attackID = attackID;
    }
}