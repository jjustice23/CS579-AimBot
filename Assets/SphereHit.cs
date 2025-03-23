using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHit : MonoBehaviour
{
    public void OnHit()
    {
        // todo: clean up ranges
        Debug.Log("test");
        Instantiate(gameObject, new Vector3(Random.Range(0,10), Random.Range(0,5), Random.Range(-5,5)), Quaternion.identity);
        Destroy(gameObject);
    }
}
