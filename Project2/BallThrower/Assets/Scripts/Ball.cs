using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum LauchPhase { HORIZONTAL, POWER, LAUNCHED}
public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject camera; 
    public GameObject force;
    public float distance_to_ARCamera = 0.15f;
    public float down_from_ARCamera = 0.025f;
    public float total_displacement_X = 0.75f;
    public float horizontal_speed = 0.5f;
    float current_displacement = 0f; 
    float current_horizontal_speed = 0.5f;
    bool first_time = true; 
    float start_Y_angle = 0.0f; 
    public float forceStrength = 5.0f;
    float prevention_time = 0.3f;
    float current_prevention_time = 0f; 
    Scrollbar power_bar;
    LauchPhase lauchPhase;
    AudioSource audioSource;


    public GameObject Nice_Shot;
    public GameObject Beast;

    [HideInInspector]
    public Rigidbody body;


    void Awake()
    {
        current_horizontal_speed = horizontal_speed; 

        lauchPhase = LauchPhase.HORIZONTAL; 

        body = gameObject.GetComponent<Rigidbody>();
        body.useGravity = false;

        if (force == null)
            force = GameObject.Find("Arrow");
        if (power_bar == null)
            power_bar = GameObject.Find("Power").GetComponent<Scrollbar>();

        camera = GameObject.Find("ARCamera"); 

        audioSource = GameObject.Find("SFX").GetComponent<AudioSource>();
        SetPowerColors(); 
        Debug.Log("Arrow starts with this Y angle:" + start_Y_angle);

        Nice_Shot = GameObject.Find("Nice_Shot");
        Beast = GameObject.Find("Nice_shot2");

    }

    // Update is called once per frame
    void Update()
    {
        if (lauchPhase == LauchPhase.HORIZONTAL)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                lauchPhase = LauchPhase.POWER;

                if (Nice_Shot.active)
                    Nice_Shot.SetActive(false);

                if (Beast.active)
                    Beast.SetActive(false);

               

            }
                
         
        }
        else if (lauchPhase == LauchPhase.POWER)
        {
          
            LaunchBallLogic();
        }
           

        if(lauchPhase != LauchPhase.LAUNCHED)
            SetWithCamera(); 
    }


    void SetWithCamera()
    {
        // Move horizontally
        if (lauchPhase == LauchPhase.HORIZONTAL)
        {
            // Calculate currrent horizontal displacement
            current_displacement += current_horizontal_speed * Time.deltaTime;
            float target_displacemente = total_displacement_X;
 
            // first time starts in the middle, so it has to travel half the way
            if (first_time)
                target_displacemente /= 2f;

            // Locate it i front but with an horizontal displacement
            transform.position = camera.transform.position + camera.transform.right * current_displacement
                + camera.transform.forward * distance_to_ARCamera;

            // When it arrives to one side, switch to the other one
            if(ArrivedOneSide(current_displacement, target_displacemente) == true)
            {
                if (first_time)
                    first_time = false;
                current_horizontal_speed *= -1f;
            }
             
        }
        else if(lauchPhase == LauchPhase.POWER) // fixed in horizontal axis
            transform.position = camera.transform.position + camera.transform.right * current_displacement
                 + camera.transform.forward * distance_to_ARCamera;

        // a bit downwards
        transform.position -= camera.transform.up * down_from_ARCamera; 
    }

    bool ArrivedOneSide(float displacement, float total_displacement)
    {
        if (current_horizontal_speed >= 0f)
        {
            if (displacement >= total_displacement)
            {
                return true;
            }
               
        }
           
        else
        {
            if (displacement <= -total_displacement)
            {
                return true;
            }

        }

        return false; 
    }

    void SetPowerColors()
    {
        float r = power_bar.value;
        float g = 1f - r;
        ColorBlock cb = new ColorBlock();
        cb.pressedColor = new Color(r, g, 0f);
        cb.highlightedColor = Color.black;
        cb.normalColor = Color.black;
        cb.selectedColor = Color.black;
        cb.colorMultiplier = 5f;
        power_bar.colors = cb;
    }

    public void LaunchBallLogic()
    {
        


        current_prevention_time += Time.deltaTime; 
        Debug.Log("Prevention time is " + current_prevention_time);

        // to not confuse the last arrow click
        if (current_prevention_time <= prevention_time)
            return;

        // Change colors
        SetPowerColors(); 

        // only launch ball when releasing
        if ((Input.GetKeyUp(KeyCode.Mouse0) == false))
            return;

        Debug.Log("Ball about to be launched!"); 

        // Body
        body.useGravity = true;

        // Launch in camera direction but with an upwards angle
        Vector3 direction = (camera.transform.forward + new Vector3(0, 1f, 0)).normalized; 
        Vector3 forceVector = direction * forceStrength * power_bar.value; 
        body.AddForce(forceVector, ForceMode.Impulse);

        // Audio
        audioSource.clip = Resources.Load("Sound/throw") as AudioClip;
        audioSource.volume = power_bar.value; 
        audioSource.Play();

        // Phase
        lauchPhase = LauchPhase.LAUNCHED;


    }



    void OnBecameInvisible()
    {
        GameObject.Find("BallManager").GetComponent<BallSpawner>().NewBall(false); 
    }

    void OnCollisionEnter(Collision col)
    {
        if (lauchPhase != LauchPhase.LAUNCHED)
            return;


        Debug.Log("hemos llegado hasta aqui");

        audioSource.volume = 1f;
        string path = "Sound/miss"; 
        if (col.collider.name == "SuccessPoint")
        {
            path = "Sound/canasta";
           
        }
            
        audioSource.clip = Resources.Load(path) as AudioClip;
        audioSource.Play(); 
    }
}
