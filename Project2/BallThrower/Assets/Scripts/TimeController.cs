using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    Text text;
    float time = 120.0f;
    GameObject finale;
  

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        finale = GameObject.Find("Final_Canvas");
        finale.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        int i_time = (int)time;
        text.text = "TIME: " + i_time.ToString() + "s";

        if(time <= 0.0f)
        {
            finale.SetActive(true);
        }
    }
}
