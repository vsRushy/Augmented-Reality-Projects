using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum LauchPhase { HORIZONTAL, POWER, LAUNCHED}
public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject camera; 
    RectTransform arrow_transform; 
    public GameObject force;
    public float arrow_rotation_speed = 10f;
    float current_arrow_rotation_speed = 0.0f; 
    public float arrow_cycle_time = 2.0f;
    float current_arrow_cycle_time = 0.0f; 
    float start_Y_angle = 0.0f; 
    public float forceStrength = 5.0f;
    float prevention_time = 0.3f;
    float current_prevention_time = 0f; 
    Scrollbar power_bar;
    Collider col; 
    LauchPhase lauchPhase;
    bool first_arrow_cycle = true;
    AudioSource audioSource; 
    [HideInInspector]
    public Rigidbody body;


    void Awake()
    {
        lauchPhase = LauchPhase.HORIZONTAL; 

        body = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>(); 
        body.useGravity = false;
      /*  body.isKinematic = true;
        col.enabled = false; */

        if (force == null)
            force = GameObject.Find("Arrow");
        if (power_bar == null)
            power_bar = GameObject.Find("Power").GetComponent<Scrollbar>();

        camera = GameObject.Find("ARCamera"); 

        audioSource = GameObject.Find("SFX").GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = Resources.Load("Sound/throw") as AudioClip;

        SetPowerColors(); 
        Debug.Log("Arrow starts with this Y angle:" + start_Y_angle); 
    }

    // Update is called once per frame
    void Update()
    {
        if (lauchPhase == LauchPhase.HORIZONTAL)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                lauchPhase = LauchPhase.POWER;
         
        }
        else if (lauchPhase == LauchPhase.POWER)
            LaunchBallLogic();

        if(lauchPhase != LauchPhase.LAUNCHED)
            SetWithCamera(); 
    }

    void SetWithCamera()
    {
        transform.position = camera.transform.position + camera.transform.forward * 0.5f;

        // a bit downwards
        transform.position -= camera.transform.up * 0.1f; 
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

        // same force upwards and forwards (45 degrees) 
        body.useGravity = true;
       /* body.isKinematic = false;
        col.enabled = true;*/

        // Launch in camera direction but with an upwards angle
        Vector3 direction = (camera.transform.forward + new Vector3(0, 1f, 0)).normalized; 
        Vector3 forceVector = direction * forceStrength * power_bar.value; 
        body.AddForce(forceVector, ForceMode.Impulse);

        // Audio
        audioSource.volume = power_bar.value; 
        audioSource.Play();

        // Phase
        lauchPhase = LauchPhase.LAUNCHED; 
    }



    void OnBecameInvisible()
    {
        GameObject.Find("BallManager").GetComponent<BallSpawner>().NewBall(false); 
    }
}
