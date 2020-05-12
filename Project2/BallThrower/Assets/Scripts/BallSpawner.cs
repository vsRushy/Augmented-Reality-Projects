﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject position;
    public GameObject uiScore;
    public GameObject arrow;

    public float max_time = 7.0f;
    private float current_time = 0.0f;

    void Start()
    {
        NewBall(false); 
    }

   /*void Update()
    {
        current_time += Time.deltaTime;
        if(current_time >= max_time)
        {
            Instantiate(prefab);
            current_time = 0.0f;
        }
    }*/

    public void NewBall(bool success)
    {
        Debug.Log("New ball to be spawned, success: " + success); 

        // Destroy previous ball
        var previousBall = GameObject.Find("Ball");
        if (previousBall)
            Destroy(previousBall); 

        // Create new ball
        var ball = Instantiate(prefab, position.transform.position, Quaternion.identity) as GameObject;
        ball.name = "Ball";
        ball.GetComponent<Ball>().enabled = true; // ... Unity is utter rubish and spawns with the script disabled

        // Score
        if (success)
        {
            var textComp = uiScore.GetComponent<Text>();
            textComp.text = (int.Parse(textComp.text) + 1).ToString(); 
        }

        // Reset Arrow
        arrow.transform.rotation = Quaternion.identity;

    }
}
