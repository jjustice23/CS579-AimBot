using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class inisibleOnAttach : MonoBehaviour
{
    [SerializeField] GameObject LeftControllerVisuals;
    [SerializeField] GameObject RightControllerVisuals;

    public void InvisOnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("entered");
        if(args.interactorObject.handedness == InteractorHandedness.Left)
        {
            LeftControllerVisuals.SetActive(false);
        }
        else
        {
            RightControllerVisuals.SetActive(false);
        }
    }

    public void VisOnDrop(SelectExitEventArgs args)
    {
        Debug.Log("exited");
        if (args.interactorObject.handedness == InteractorHandedness.Left)
        {
            LeftControllerVisuals.SetActive(true);
        }
        else
        {
            RightControllerVisuals.SetActive(true);
        }

    }
}
