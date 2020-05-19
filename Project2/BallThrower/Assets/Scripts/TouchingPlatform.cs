using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingPlatform : MonoBehaviour
{
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Ball")
        {
            GameObject.Find("Ball").GetComponent<Ball>().ten_points = true;
        }

    }
}
