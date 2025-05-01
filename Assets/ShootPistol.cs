//using System;
using System.Collections;
using Unity.XR.CoreUtils;

//using System.Collections.Generic;
//using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;
namespace UnityEngine.XR.Content.Interaction
{
    public class ShootPistol : MonoBehaviour
    {
        [SerializeField] private AudioClip shellDropClip;
        [SerializeField] private VisualEffect muzzleFlash;
        [SerializeField] private GameObject muzzleLight;
        [SerializeField] private GameObject bulletHolePrefab;
        [SerializeField] private GameObject bulletHoleContainer;
        [SerializeField] private float destroyDelay;
        [SerializeField] private GameObject AimDirection;
        private AudioSource gunAudio;

        LightFlash LightFlashScript;

        private void Start()
        {
            LightFlashScript = muzzleLight.GetComponent<LightFlash>();
        }

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
            //TriggerHaptics(0.7f, 0.1f); // Call for haptics
            muzzleFlash.Play();
            LightFlashScript.FlashLight();

            Vector3 gunPosition = AimDirection.transform.position; // adjust to "Sight" maybe?
            //Vector3 gunForwardDirection = transform.right*-1; // a little hacky, but seems to work for now
            Vector3 gunForwardDirection = AimDirection.transform.forward;
            int interactableLayer = LayerMask.GetMask("Target");
            int roomLayer = LayerMask.GetMask("env");

            Ray ray = new Ray(gunPosition, gunForwardDirection);
            RaycastHit hit;

            //Debug.DrawRay(gunPosition, gunForwardDirection*10, Color.red, 20);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                hit.collider.gameObject.GetComponent<SphereHit>().OnHit();
            } else if (Physics.Raycast(ray, out hit, Mathf.Infinity, roomLayer))
            {
                float positionMultiplier = .5f;
                float spawnX = hit.point.x - ray.direction.x * positionMultiplier;
                float spawnY = hit.point.y - ray.direction.y * positionMultiplier;
                float spawnZ = hit.point.z - ray.direction.z * positionMultiplier;
                Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);

                GameObject bulletHole = Instantiate(bulletHolePrefab, spawnPos, Quaternion.identity);
                Quaternion targetRotation = Quaternion.LookRotation(ray.direction);

                bulletHole.transform.rotation = targetRotation;
                bulletHole.transform.SetParent(bulletHoleContainer.transform);
                bulletHole.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
                Destroy(bulletHole, destroyDelay);
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