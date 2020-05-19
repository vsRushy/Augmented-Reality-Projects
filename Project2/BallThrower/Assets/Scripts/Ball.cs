using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum LauchPhase { HORIZONTAL, POWER, LAUNCHED}
public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject arrow;
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
        body.isKinematic = true; 
        body.useGravity = false;
        col.enabled = false; 

        if (arrow == null)
            arrow = GameObject.Find("Arrow");
        if (force == null)
            force = GameObject.Find("Arrow");
        if (power_bar == null)
            power_bar = GameObject.Find("Power").GetComponent<Scrollbar>();

        camera = GameObject.Find("ARCamera"); 

        arrow_transform = arrow.GetComponent<RectTransform>(); 
        start_Y_angle = arrow_transform.rotation.eulerAngles.y;
        current_arrow_rotation_speed = arrow_rotation_speed;

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
            RotateArrow();

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
        arrow.transform.position = camera.transform.position + camera.transform.forward * 0.5f;

        // a bit downwards
        transform.position -= new Vector3(0f, 0.2f, 0f);
        arrow.transform.position -= new Vector3(0f, 0.5f, -0.2f);
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

        // Arrow
        arrow.GetComponent<Image>().enabled = false; 
   
        // Math
        float alpha = arrow_transform.rotation.eulerAngles.y + 180f;
        float omega = 90f - alpha;
        float side = arrow_transform.rect.width;
        float result = (Mathf.Sin(alpha * Mathf.Deg2Rad) * side) / (Mathf.Sin(omega * Mathf.Deg2Rad));

        // same force upwards and forwards (45 degrees) 
        body.useGravity = true;
        body.isKinematic = false;
        col.enabled = true; 
        Vector3 forceVector = new Vector3(result, side, side).normalized * forceStrength * power_bar.value; 
        body.AddForce(forceVector, ForceMode.Impulse);

        // Audio
        audioSource.volume = power_bar.value; 
        audioSource.Play();

        // Phase
        lauchPhase = LauchPhase.LAUNCHED; 
    }


    void RotateArrow()
    {
        current_arrow_cycle_time += Time.deltaTime; 
        arrow_transform.Rotate(Vector3.up, current_arrow_rotation_speed * Time.deltaTime, Space.World);

        float total_time = current_arrow_cycle_time;
        if (first_arrow_cycle)
            total_time *= 2; 

        if (total_time >= arrow_cycle_time)
        {
            if (first_arrow_cycle)
                first_arrow_cycle = false; 

            current_arrow_cycle_time = 0.0f;
            current_arrow_rotation_speed *= -1;
        }

    }


    void OnBecameInvisible()
    {
        GameObject.Find("BallManager").GetComponent<BallSpawner>().NewBall(false); 
    }
}
