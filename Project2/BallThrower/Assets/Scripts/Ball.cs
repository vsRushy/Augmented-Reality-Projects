using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject arrow;
    public float forceStrength = 5.0f; 
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
        if (clicked && Input.GetKeyUp(KeyCode.Mouse0))
        {
            body.useGravity = true;
            float Z = arrow.transform.localRotation.eulerAngles.z;

            if (Z < 0)
                Z = 360 + Z;


            float X = 0.0f, Y = 0.0f;
            switch (Z)
            {
                case float n when (Z <= 90):
                    X = Mathf.Cos(Z);
                    Y = Mathf.Sin(Z);
                    break; 
                case float n when ((Z > 90) && (Z <= 180)):
                    X = -Mathf.Cos(Z);
                    Y = Mathf.Sin(Z);
                    break;
                case float n when ((Z > 180) && (Z <= 270)):
                    X = -Mathf.Cos(Z);
                    Y = -Mathf.Sin(Z);
                    break; 
                case float n when (Z > 270):
                    X = Mathf.Cos(Z);
                    Y = -Mathf.Sin(Z);
                    break;
            }


            Vector3 forceVector = new Vector3(X, Y, 0) * forceStrength; 
            body.AddForce(forceVector, ForceMode.Impulse); 
            arrow.transform.rotation = Quaternion.identity; 
            arrow.SetActive(false);

            Debug.Log("Ball launched with Z rotation: " + Z + "and force: " + forceVector); 
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
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(arrow.transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 

        }
    }
}
