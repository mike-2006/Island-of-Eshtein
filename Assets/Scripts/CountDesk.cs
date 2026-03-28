using UnityEngine;

public class CountDesk : PickUp
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Bag bag = other.GetComponent<Bag>();

        if (bag != null)
        {
            bag.Add_Desk(1);
        }
    }
}
