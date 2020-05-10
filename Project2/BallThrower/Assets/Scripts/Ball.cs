using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject arrow;
    public float forceStrength = 0.05f; 
    private Rigidbody body;
    bool clicked = false;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        SetArrow(); 

        // Activate rigidbody and impulse it with the arrow direction 
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            body.useGravity = true;
            Vector3 forceVector = new Vector3(arrowRotation, arrowRotation - 90, 0) * forceStrength; 
            body.AddForce(forceVector, ForceMode.Impulse); // TODO
            arrow.transform.rotation = Quaternion.identity; 
            arrow.SetActive(false);

            Debug.Log("Ball launched with angle in z " + arrowRotation + "and force: " + forceVector); 
        }
          
    }

    void SetArrow()
    {
        if(!clicked)
        {
            // Set arrow as visible when clicking
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                clicked = true;
                arrow.SetActive(true); 
            }
        }
        else
        {
            // Mouse position
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;

            // Angle between mouse and arrow
            float angle = Mathf.Atan2(mousePos.y - arrow.transform.position.y, mousePos.x - arrow.transform.position.x);
            angle *= (180 / Mathf.PI); 

            // Rotate arrow
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

        }
    }
}
