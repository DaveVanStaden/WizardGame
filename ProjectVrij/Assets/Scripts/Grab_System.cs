using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab_System : MonoBehaviour
{
    public float sphereCastRadius = 0.5f;
    public int interactableLayerIndex;
    //private Vector3 raycastPos;
    public GameObject lookObject;
    private Grabable_Object physicsObject;
    //public Camera mainCamera;
    public Collider playerColl;

    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;
    private Collider pickupColl;

    [SerializeField] public float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 10f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    public float rotationSpeed = 100f;
    Quaternion lookRot;

    private void Start()
    {
        //mainCamera = Camera.main;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pickupParent.position, 0.1f);
    }

    void Update()
    {
        //raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCastRadius, transform.TransformDirection(Vector3.forward), out hit, maxDistance, 1 << interactableLayerIndex))
        {
            lookObject = hit.collider.transform.root.gameObject;
        } else
        {
            lookObject = null;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentlyPickedUpObject == null)
            {
                if (lookObject != null)
                {
                    PickupObject();
                }
            } else
            {
                BreakConnection();
            }
        }
        if (currentlyPickedUpObject != null && currentDist > maxDistance) BreakConnection();

    }

    private void FixedUpdate()
    {
        if (currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed *= Time.fixedDeltaTime;
            Vector3 direction = pickupParent.position - pickupRB.position;
            pickupRB.velocity = direction.normalized * currentSpeed;

            //lookRot = Quaternion.LookRotation(mainCamera.transform.position - currentlyPickedUpObject.transform.position);
            //lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            pickupRB.MoveRotation(Quaternion.LookRotation(transform.forward, transform.up));
        }
    }


    public void BreakConnection()
    {
        pickupRB.constraints = RigidbodyConstraints.None;
        Physics.IgnoreCollision(pickupColl, playerColl, false);
        currentlyPickedUpObject = null;
        physicsObject.pickedUp = false;
        currentDist = 0;
    }

    public void PickupObject()
    {
        physicsObject = lookObject.GetComponentInChildren<Grabable_Object>();
        currentlyPickedUpObject = lookObject;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        pickupColl = currentlyPickedUpObject.GetComponent<Collider>();
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        Physics.IgnoreCollision(pickupColl, playerColl, true);
        physicsObject.playerInteractions = this;
        StartCoroutine(physicsObject.PickUp());
    }
}
