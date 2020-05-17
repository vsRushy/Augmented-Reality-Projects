using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject position;
    public GameObject uiScore;
    public GameObject arrow;
    Quaternion arrow_start_rotation;
    Scrollbar power_bar; 

    public float max_time = 7.0f;
    private float current_time = 0.0f;

    void Start()
    {
        arrow_start_rotation = arrow.GetComponent<RectTransform>().rotation; 
        NewBall(false);
        Debug.Log("Arrow detected with rotation: " + arrow_start_rotation);
        power_bar = GameObject.Find("Power").GetComponent<Scrollbar>();
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

        // Arrow
        arrow.GetComponent<RectTransform>().rotation = arrow_start_rotation;
        arrow.GetComponent<Image>().enabled = true;

    }



   
}
