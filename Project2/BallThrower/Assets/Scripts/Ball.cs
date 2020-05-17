using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum LauchPhase { HORIZONTAL, POWER}
public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject arrow;
    RectTransform arrow_transform; 
    public GameObject force;
    public float arrow_rotation_speed = 10f;
    float current_arrow_rotation_speed = 0.0f; 
    public float arrow_cycle_time = 2.0f;
    float current_arrow_cycle_time = 0.0f; 
    float start_Y_angle = 0.0f; 
    public float forceStrength = 5.0f;
    bool first_arrow_cycle = true; 
    [HideInInspector]
    public Rigidbody body;
    LauchPhase lauchPhase; 

    void Awake()
    {
        lauchPhase = LauchPhase.HORIZONTAL; 

        body = gameObject.GetComponent<Rigidbody>();
        body.useGravity = false; 

        if (arrow == null)
            arrow = GameObject.Find("Arrow");
        if (force == null)
            force = GameObject.Find("Arrow");

        arrow_transform = arrow.GetComponent<RectTransform>(); 
        start_Y_angle = arrow_transform.rotation.eulerAngles.y;
        current_arrow_rotation_speed = arrow_rotation_speed;

        Debug.Log("Arrow starts with this Y angle:" + start_Y_angle); 
    }

    // Update is called once per frame
    void Update()
    {
        if (lauchPhase == LauchPhase.HORIZONTAL)
        {
            RotateArrow();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Phase
                lauchPhase = LauchPhase.POWER;

                // TODO: comment this
                LaunchBall(); 
            }
        }
        else if (lauchPhase == LauchPhase.POWER)
        {

        }

    }

    void LaunchBall()
    {
        arrow.GetComponent<Image>().enabled = false; // this bugs the arrow in the next ball XD

        body.useGravity = true;

        float alpha = arrow_transform.rotation.eulerAngles.y + 180f;
        float omega = 90f - alpha;
        float side = arrow_transform.rect.width;
        float result = (Mathf.Sin(alpha * Mathf.Deg2Rad) * side) / (Mathf.Sin(omega * Mathf.Deg2Rad));

        Vector3 forceVector = new Vector3(result, side, side).normalized * forceStrength; // same force upwards and forwards (45 degrees) 
        body.AddForce(forceVector, ForceMode.Impulse);
       
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
