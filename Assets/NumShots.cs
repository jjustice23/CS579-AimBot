using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumShots : MonoBehaviour
{
    [SerializeField] GameObject HitData;
    private SphereHit hd;
    private int TotalShots;

    public void Start()
    {
        hd = HitData.GetComponent<SphereHit>();
    }

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
