using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Ball")
            GameObject.Find("BallManager").GetComponent<BallSpawner>().NewBall(true);

    }
}
