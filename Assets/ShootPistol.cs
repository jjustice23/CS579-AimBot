//using System;
using System.Collections;
//using System.Collections.Generic;
//using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;
namespace UnityEngine.XR.Content.Interaction
{
    public class ShootPistol : MonoBehaviour
    {
        [SerializeField] private AudioClip shellDropClip;
        private AudioSource gunAudio;

        private void Awake()
        {
            gunAudio = GetComponent<AudioSource>(); //gunshot sound
        }
        public void Fire()
        {
            if (gunAudio != null && gunAudio.clip != null)
            {
                gunAudio.PlayOneShot(gunAudio.clip); //play shot soudn
            }

            if (shellDropClip != null)
            {
                StartCoroutine(PlayShellDelayed(0.5f)); //calls subroutine to play shell casing sound
            }
            TriggerHaptics(0.7f, 0.1f); // Call for haptics
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
        private IEnumerator PlayShellDelayed(float delay)
        {
            yield return new WaitForSeconds(delay); // wait for the delay
            gunAudio.PlayOneShot(shellDropClip); //play the shell casing sound
        }
        
        private void TriggerHaptics(float amplitude, float duration) 
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand); // 
            if (device.isValid && device.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0, amplitude, duration);
            }
        }
        
    }
}