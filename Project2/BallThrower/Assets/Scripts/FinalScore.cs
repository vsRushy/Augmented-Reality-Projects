using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    Text final_score;

    // Start is called before the first frame update
    void Start()
    {
        final_score.text = ScoreManager.score.ToString();
    }
}

