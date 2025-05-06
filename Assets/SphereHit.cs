using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Content.Interaction;

public class SphereHit : MonoBehaviour
{
    // access modifiers are a mess, could clean it up, but I probably won't
    public GameObject floor;
    public GameObject wall;
    public GameObject room;

    //public float TimerDefault;
    public GameObject StartButton;
    public GameObject Shots;
    public TextMeshProUGUI Stats;
    public TextMeshProUGUI TimerText;
    public GameObject TimerSlider;
    

    private Slider slider;
    private int TargetsHit;
    private float HitRatio; // find way to track the number of times the user "fires" the gun
    private StartButtonScript ButtonStatus;
    //private XRPushButton Pushed;
    private NumShots TotalShots;    // !!NOTE: CURRENTLY WILL ONLY WORK FOR 1 GUN TYPE NEED TO FIND MORE FLEXIBLE SOLUTION!!
    [System.NonSerialized]public bool GameStarted = false;
    private float Timer;
    private AudioSource TimerEnd;

    public Toggle MovingModeToggle;
    public float moveSpeed = 2f; // you can tweak this in the inspector
    private bool isMoving = false;
    private Vector3 moveDirection;

    private ParticleSystem particles;
    private float destroyDelay = .15f;
    private Renderer targetRenderer;

    private AudioSource hitAudio; 


    private void InstatiateTarget(int TargetsHit, float HitRatio, bool GameStarted, float Timer)
    {
        this.TargetsHit = TargetsHit;
        this.HitRatio = HitRatio;
        this.GameStarted = GameStarted;
        this.Timer = Timer;
    }

    private void Start()
    {
        ButtonStatus = StartButton.GetComponent<StartButtonScript>();
        //Pushed = StartButton.GetComponent<XRPushButton>();
        TotalShots = Shots.GetComponent<NumShots>();
        slider = TimerSlider.GetComponent<Slider>();
        TimerEnd = GetComponent<AudioSource>();
        hitAudio = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
        targetRenderer = GetComponent<Renderer>();



        if (MovingModeToggle != null)
        {
            isMoving = MovingModeToggle.isOn;
            MovingModeToggle.onValueChanged.AddListener(OnToggleChanged);
        }
        SetInitialMoveDirection();
    }

    private void Update()
    {
        if (GameStarted)
        {
            //Debug.Log(Timer);
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
                UpdateTimerText();
            }
            else if (Timer <= 0)
            {
                //need to reset button height
                TimerEnd.PlayOneShot(TimerEnd.clip);
                GameStarted = false;
                ButtonStatus.OnClick();
                ButtonStatus.ToggleButton();
                // note TargetHits decremented to ignore first target that starts game
                //Stats.text = TargetsHit-1 + " Targets Hit and " + ((float)(TargetsHit-1)/(float)TotalShots.get())*100 + "% Hit Ratio";  // should probably use HitRatio var to be more idiomatic, but I cant be bothered
                UpdateStatsText(); // probably unneeded
            }
            if (isMoving)
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                // Check if we hit a wall — if so, bounce back
                float floorWidth = floor.transform.localScale.z * room.transform.localScale.z;
                float zPos = transform.position.z;
                float sphereRadius = transform.localScale.x / 2f; 


                if (zPos <= (-floorWidth / 2)+sphereRadius || zPos >= (floorWidth / 2)-sphereRadius)
                {
                    moveDirection = -moveDirection;
                }
            }
        }
    }

    public void OnHit()
    {
        particles.Play();
        if (GameStarted)
        {
            TargetsHit++;
            UpdateStatsText();
        }

        if (ButtonStatus.started && !GameStarted)
        {
            TotalShots.set(0);
            Timer = slider.value;
            //ButtonStatus.started = false;
            ButtonStatus.ToggleButton();
            GameStarted = true;
            HitRatio = 0.0f;
            TargetsHit = 0;
            UpdateStatsText();
            UpdateTimerText();
        }
        if (!isMoving || !GameStarted){SpawnTarget();}
        if (hitAudio != null)
        {
            hitAudio.Play();
        }

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
        //GameObject NewSphere = Instantiate(gameObject, new Vector3(x, y, z/2), Quaternion.identity);
        GameObject NewSphere = Instantiate(gameObject, new Vector3(x, y, z), Quaternion.identity);
        SphereHit SphereScript = NewSphere.GetComponent<SphereHit>();
        SphereScript.InstatiateTarget(TargetsHit, HitRatio, GameStarted, Timer);

        targetRenderer.enabled = false;
        particles.Play();
        //Destroy(gameObject);
        StartCoroutine(DestroyTargetAfterEffect());
    }

    public void UpdateStatsText()
    {
        float ratio;
        if(TargetsHit == 0)
        {
            ratio = 1.0f;
        }
        else
        {
            ratio = ((float)(TargetsHit - 1) / (float)TotalShots.get());
        }
        Stats.text = "Targets Hit: " + (TargetsHit) + "\nHit Ratio: " + (ratio * 100) + "%";  // should probably use HitRatio var to be more idiomatic, but I cant be bothered
    }

    private void UpdateTimerText()
    {
        if(Timer >= 0) TimerText.text = "Timer: " + Mathf.Floor(Timer);
    }

    private void SetInitialMoveDirection()
    {
        moveDirection = Vector3.forward; 
    }

    private void OnToggleChanged(bool value)
    {
        isMoving = value;
    }



    private IEnumerator DestroyTargetAfterEffect()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }

}
