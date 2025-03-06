using UnityEngine;

public class TargetDummy : MonoBehaviour, IHittable
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void hit(Attack attack)
    {
        Debug.Log($"Damge: {attack.damage}\n Stun: {attack.stun}\n Revenge: {attack.revenge}\n ID: {attack.attackID}\n Special: {attack.isSpecial}");
    }
}
