using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InScreenClamp : MonoBehaviour
{
    public Camera MainCamera;
    public SpriteRenderer spriteOfObject;
    public float minYValue;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    [HideInInspector]
    public bool isPlayerOnRiver = false;

    void Start()
    {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = spriteOfObject.bounds.extents.x; 
        objectHeight = spriteOfObject.bounds.extents.y; 
    }

    
    void LateUpdate()
    {
        if (!isPlayerOnRiver)
        {
            Vector3 viewPos = transform.position;
            viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
            viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
            transform.position = viewPos;
        }
        else
        {
            Vector3 playerPos = transform.position;
            if(playerPos.x < screenBounds.x * -1 - objectWidth || playerPos.x > screenBounds.x + objectWidth )
            {
                Debug.Log("Death");
            }

        }

        if(transform.position.y < minYValue)
        {
            transform.position = new Vector3(transform.position.x,minYValue,transform.position.z);
        }
    }
}
