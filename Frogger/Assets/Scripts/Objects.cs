using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public float movementSpeed;
    public Vector2 startAndEndOfRoad;
    void Update()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
       
        if (transform.localPosition.x > startAndEndOfRoad.y)
        {
            transform.localPosition = new Vector3(startAndEndOfRoad.x, transform.localPosition.y, transform.localPosition.z);
        }

    }
}
