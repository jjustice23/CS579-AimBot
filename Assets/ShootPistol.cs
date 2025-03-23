//using System;
//using System.Collections;
//using System.Collections.Generic;
//using JetBrains.Annotations;
//using UnityEngine;
namespace UnityEngine.XR.Content.Interaction
{
    public class ShootPistol : MonoBehaviour
    {
        public void Fire()
        {
            Vector3 gunPosition = transform.position; // adjust to "Sight" maybe?
            Vector3 gunForwardDirection = transform.right*-1; // a little hacky, but seems to work for now
            int interactableLayer = LayerMask.GetMask("Target");

            Ray ray = new Ray(gunPosition, gunForwardDirection);
            RaycastHit hit;

            //Debug.DrawRay(gunPosition, gunForwardDirection*10, Color.red, 20);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                hit.collider.gameObject.GetComponent<SphereHit>().OnHit();
            }
        }
    }
}