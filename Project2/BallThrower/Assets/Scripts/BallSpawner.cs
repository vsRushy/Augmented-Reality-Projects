using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject position;
    public GameObject uiScore;
    Scrollbar power_bar;


    public GameObject Nice_Shot;
    public GameObject Beast;

    AudioSource audioSource;

    public float max_popUp_time = 3.0f;
    private float current_time = 0.0f;
    bool count = false; 

    public 
    void Start()
    {
        NewBall(false);
        power_bar = GameObject.Find("Power").GetComponent<Scrollbar>();

     /*   Nice_Shot = GameObject.Find("Nice_Shot");
        Beast = GameObject.Find("Nice_shot2");*/

        Nice_Shot.SetActive(false);
        Beast.SetActive(false);

        audioSource = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

   void Update()
    {
        if(count)
        {
            current_time += Time.deltaTime;
            if (current_time >= max_popUp_time)
            {
                PopUpEnd(); 
            }
        }

    }

    void PopUpEnd()
    {
        SetPopUpsActive(false);
        current_time = 0.0f;
        count = false;
    }

    public void NewBall(bool success)
    {
        Debug.Log("New ball to be spawned, success: " + success);

        // Destroy previous ball
        var previousBall = GameObject.Find("Ball");
        if (previousBall)
            Destroy(previousBall);

        // Score
        if (success)
        {
            var textComp = uiScore.GetComponent<Text>();
            textComp.text = (int.Parse(textComp.text) + 1).ToString();

            audioSource.volume = 1f;
            string path = "Sound/canasta";

            audioSource.clip = Resources.Load(path) as AudioClip;
            audioSource.Play();

            int h = Random.Range(0, 2);
            if (h == 0)
            {
                Nice_Shot.SetActive(true);
                Debug.Log("NICE SHOT POP-UP"); 
            }
            else
            {
                Beast.SetActive(true);
                Debug.Log("BEAST POP-UP");
            }
            

            // start counting
            count = true; 

            return; 
        }

        // Create new ball
        var ball = Instantiate(prefab, position.transform.position, Quaternion.identity) as GameObject;
        ball.name = "Ball";
        ball.GetComponent<Ball>().enabled = true; // ... Unity is utter rubish and spawns with the script disable
    }

    public void SetPopUpsActive (bool active)
    {
        Nice_Shot.SetActive(active);
        Beast.SetActive(active); 
    }


   
}
