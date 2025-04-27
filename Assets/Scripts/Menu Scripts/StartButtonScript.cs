using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour
{
    [System.NonSerialized]public bool started;
    private Button button;
    private ColorBlock cb;

    void Start()
    {
        started = false;
        button = GetComponent<Button>();
        cb = button.colors;
    }

    public void OnClick()
    {
        started = !started;
        cb.normalColor = (started) ? Color.green : Color.red;

        button.colors = cb;
    }

    public void ToggleButton()
    {
        button.interactable = !button.interactable;
    }
}
