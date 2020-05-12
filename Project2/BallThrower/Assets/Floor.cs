using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 posVector;
    Vector3 rotationVector;

    void Start()
    {
        posVector = new Vector3(0, 0, transform.position.z);
        rotationVector = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 previousPos = transform.position;
        
        // Position: keep Z
        posVector.x = transform.position.x;
        posVector.y = transform.position.y;
        transform.position = posVector;
      //  transform.parent.position += (transform.position - previousPos);

       /* // Rotation: only rotate in Z
        transform.rotation = Quaternion.identity; */

    }
}
