using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SphereHit : MonoBehaviour
{
    // access modifiers are a mess, could clean it up, but I probably won't
    public GameObject floor;
    public GameObject wall;
    public GameObject room;

    public float TimerDefault;
    public GameObject StartButton;
    public GameObject Shots;
    public TextMeshProUGUI Stats;

    private int TargetsHit;
    private float HitRatio; // find way to track the number of times the user "fires" the gun
    private script ButtonStatus;
    private XRPushButton Pushed;
    private NumShots TotalShots;    // !!NOTE: CURRENTLY WILL ONLY WORK FOR 1 GUN TYPE NEED TO FIND MORE FLEXIBLE SOLUTION!!
    private bool GameStarted = false;
    private float Timer;

    private void InstatiateTarget(int TargetsHit, float HitRatio, bool GameStarted, float Timer)
    {
        this.TargetsHit = TargetsHit;
        this.HitRatio = HitRatio;
        this.GameStarted = GameStarted;
        this.Timer = Timer;
    }

    private void Start()
    {
        ButtonStatus = StartButton.GetComponent<script>();
        Pushed = StartButton.GetComponent<XRPushButton>();
        TotalShots = Shots.GetComponent<NumShots>();
    }

    private void Update()
    {
        if (GameStarted)
        {
            //Debug.Log(Timer);
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
            else if (Timer <= 0)
            {
                //need to reset button height
                GameStarted = false;
                // note TargetHits decremented to ignore first target that starts game
                Stats.text = TargetsHit-1 + " Targets Hit and " + ((float)(TargetsHit-1)/(float)TotalShots.get())*100 + "% Hit Ratio";  // should probably use HitRatio var to be more idiomatic, but I cant be bothered
            }
        }
    }

    public void OnHit()
    {

        if (GameStarted)
        {
            TargetsHit++;
        }

        if (ButtonStatus.started && !GameStarted)
        {
            TotalShots.set(0);
            Timer = TimerDefault;
            ButtonStatus.started = false;
            GameStarted = true;
            HitRatio = 0.0f;
            TargetsHit = 0;
        }

        SpawnTarget();
    }

    private void SpawnTarget()
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

        float x = Random.Range(0 + sphereRadius, (floorLen / 2) - sphereRadius);
        float y = Random.Range(0 + sphereRadius, wallHeight - sphereRadius);
        float z = Random.Range((-1 * (floorWidth / 2)) + sphereRadius, (floorWidth / 2) - sphereRadius);
        GameObject NewSphere = Instantiate(gameObject, new Vector3(x, y, z), Quaternion.identity);
        SphereHit SphereScript = NewSphere.GetComponent<SphereHit>();
        SphereScript.InstatiateTarget(TargetsHit, HitRatio, GameStarted, Timer);

        Destroy(gameObject);
    }
}
