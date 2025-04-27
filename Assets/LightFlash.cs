using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour
{
    [SerializeField] private float disableTime;

    private Light MuzzleLight;

    private void Start()
    {
        MuzzleLight = GetComponent<Light>();
        MuzzleLight.enabled = false;
    }
    public void FlashLight()
    {
        StartCoroutine(MuzzleFlashLightCR());
    }

    private IEnumerator MuzzleFlashLightCR()
    {
        MuzzleLight.enabled = true;
        yield return new WaitForSeconds(disableTime);
        MuzzleLight.enabled = false;
    }
}
