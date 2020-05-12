using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketTargetMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public GameObject downPos;
    public GameObject topPos;
    private Vector3 currentSpeedVector;
    private Vector3 targetPos; 

    // Start is called before the first frame update
    void Start()
    {
        SwitchMovement(true, downPos.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += currentSpeedVector * Time.deltaTime; 

        if(currentSpeedVector.y < 0)
        {
            if(transform.position.y <= targetPos.y)
            {
                transform.position = targetPos;
                SwitchMovement(false, topPos.transform.position); 
              
            }
        }
        else
        {
            if (transform.position.y >= targetPos.y)
            {
                transform.position = targetPos;
                SwitchMovement(true, downPos.transform.position);

            }
        }
        
    }

    void SwitchMovement(bool down, Vector3 objectPos)
    {
        SetSpeedVector(down);
        SetTargetPos(objectPos); 
    }

    void SetTargetPos(Vector3 objectPos)
    {
        targetPos = objectPos;
        targetPos.x = transform.position.x;
        targetPos.z = transform.position.z; 
    }

    void SetSpeedVector(bool down)
    {
        if(down)
            currentSpeedVector = new Vector3(0, -speed, 0);
        else
            currentSpeedVector = new Vector3(0, speed, 0);

    }
}
