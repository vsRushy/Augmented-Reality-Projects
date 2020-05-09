using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;

    public float max_time = 7.0f;
    private float current_time = 0.0f;

    void Start()
    {
        
    }

    void Update()
    {
        current_time += Time.deltaTime;
        if(current_time >= max_time)
        {
            Instantiate(prefab);
            current_time = 0.0f;
        }
    }
}
