using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrabObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the selectEntered and selectExited events directly
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Get the hand/controller transform when grabbed
        Transform handTransform = args.interactorObject.transform;


        // Set the object’s position and rotation to match the hand
        transform.SetPositionAndRotation(handTransform.position, handTransform.rotation);
        //transform.parent = handTransform.parent;


        // Optionally, set isKinematic on the Rigidbody to ensure it doesn't get affected by physics while grabbed
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Reset isKinematic on the Rigidbody when released
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void Update()
    {
        // While grabbed, make sure the object follows the hand's position and rotation
        if (grabInteractable.isSelected)
        {
            Transform handTransform = grabInteractable.interactorsSelecting[0].transform;
            transform.SetPositionAndRotation(handTransform.position, handTransform.rotation);
        }
    }
}