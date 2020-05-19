using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessPoint : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer meshRenderer; 
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.enabled = false; // vuforia changes this!
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Ball")
        {
            GameObject.Find("BallManager").GetComponent<BallSpawner>().NewBall(true);
            ++ScoreManager.score;
        }

    }
}
