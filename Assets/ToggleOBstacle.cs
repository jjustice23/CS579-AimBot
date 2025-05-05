using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjToggle : MonoBehaviour
{
    public Toggle boxToggle;
    public GameObject boxStack; 
    void Start()
    {
        if(boxToggle != null && boxStack != null)
        {
            boxToggle.isOn = boxStack.activeSelf;
            boxToggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    void OnToggleChanged(bool isOn)
    {
        if(boxStack != null)
        {
            boxStack.SetActive(isOn);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
