using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    public GameObject attacker;
    private IAttacker attackerI;
    
    public attackerIdentifier ID;

    public bool active;

    private void Awake()
    {
        attackerI = attacker.GetComponent<IAttacker>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (active && attackerI != null)
        {
            attackerI.attackCollision(ID, collision);
        }
    }
}

public interface IAttacker{
    public void attackCollision(attackerIdentifier ID, Collider collision);
}

public enum attackerIdentifier
{
    None = 0,
    RightFoot = 1,
    LeftFoot = 2,
    RightFist = 3,
    LeftFist = 4
};
