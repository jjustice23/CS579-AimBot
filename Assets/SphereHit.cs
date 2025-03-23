using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHit : MonoBehaviour
{
    public GameObject floor;
    public GameObject wall;
    public GameObject room;
    public void OnHit()
    {
        /* condtions: zero needs to be the centerpoint of room, sphereRadius should not be larger than half any room dimension, 
                      and the floor should be leveled so the top is at y=0

           other notes: x=0 to the positive direction is considered the "target area" and anything to the negative direction is the "player area"
                        
        */

        // if these are met, the room should be scalable (potentially to modify difficulty), but needs to be tested

        float sphereRadius = transform.localScale.x / 2;  // seems correct in in concept, debug at some point
        float floorLen = floor.transform.localScale.x * room.transform.localScale.x;
        float floorWidth = floor.transform.localScale.z * room.transform.localScale.z;
        float wallHeight = wall.transform.localScale.y * room.transform.localScale.y;
        // todo: clean up ranges
        Debug.Log("test");

        float x = Random.Range(0 + sphereRadius, (floorLen / 2) - sphereRadius);
        float y = Random.Range(0 + sphereRadius, wallHeight - sphereRadius);
        float z = Random.Range((-1 * (floorWidth / 2)) + sphereRadius, (floorWidth / 2) - sphereRadius);
        Instantiate(gameObject, new Vector3(x, y, z), Quaternion.identity);

        Destroy(gameObject);
    }
}
