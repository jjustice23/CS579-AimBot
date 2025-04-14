using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumShots : MonoBehaviour
{
    private int TotalShots;

    public int get()
    {
        return TotalShots;
    }

    public void set(int NumShots)
    {
        TotalShots = NumShots;
    }

    public void Increment()
    {
        TotalShots++;
    }
}
