using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile_Input : MonoBehaviour
{
    public GameObject particle;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray))
                {
                    // Create a particle if hit
                    Instantiate(particle, Camera.main.ScreenToWorldPoint(touch.position), transform.rotation);
                }
            }
        }
    }
}
