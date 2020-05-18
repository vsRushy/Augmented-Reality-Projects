using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    Text text;
    float time = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        int i_time = (int)time;
        text.text = "TIME: " + i_time.ToString() + "s";

        if(time <= 0.0f)
        {
            // Change scene
        }
    }
}
