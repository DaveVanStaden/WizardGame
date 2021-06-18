using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable_Object : MonoBehaviour
{
    public new GameObject gameObject;
    public float waitOnPickup = 0.2f;
    public float breakForce = 35f;
    [HideInInspector] public bool pickedUp = false;
    [HideInInspector] public Grab_System playerInteractions;

    private void Start()
    {
        gameObject = this.GetComponent<GameObject>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(pickedUp)
        {
            if(collision.relativeVelocity.magnitude > breakForce)
            {
                playerInteractions.BreakConnection();
            }
        }

    }
    public IEnumerator PickUp()
    {
        yield return new WaitForSecondsRealtime(waitOnPickup);
        pickedUp = true;
    }
}
