using UnityEngine;

public class passHitToParent : MonoBehaviour, IHittable
{
    private Component parent_hittable;
    public void hit(Attack attack)
    {
        parent_hittable.GetComponent<IHittable>().hit(attack);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent_hittable = (Component)transform.root.gameObject.GetComponent<IHittable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
