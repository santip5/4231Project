using System;
using UnityEngine;

public interface IHittable
{
    void hit(attack attack);
}

[Serializable]
public struct attack
{
    public int damage;
    public int stun;
    public int revenge;
    public bool isSpecial;
    public int attackID;
}