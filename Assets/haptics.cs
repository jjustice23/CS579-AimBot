using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class haptics : MonoBehaviour
{
    [Range(0,1)]
    public float intensity;
    public float duration;


    public void SendHapticFeedback(ActivateEventArgs args)
    {
        InteractorHandedness test = args.interactorObject.handedness;
        if (test == InteractorHandedness.Left)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (device.isValid && device.TryGetHapticCapabilities(out UnityEngine.XR.HapticCapabilities capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0, intensity, duration);
            }

        }
        else
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (device.isValid && device.TryGetHapticCapabilities(out UnityEngine.XR.HapticCapabilities capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0, intensity, duration);
            }
        }
    }
}
