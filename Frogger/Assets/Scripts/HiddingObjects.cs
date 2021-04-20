using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddingObjects : MonoBehaviour
{
    public float durationOfInvisibility;
    public float timeBeforeHidding;
    public float timeBetweenInvisibility;
    float durationOfInvisibilityCD;
    float timeBeforeHiddingCD;
    float timeBetweenInvisibilityCD;
    [HideInInspector]
    public bool isInvisible = false;
    Animator animator;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        durationOfInvisibilityCD = durationOfInvisibility;
        timeBeforeHiddingCD = timeBeforeHidding;
        timeBetweenInvisibilityCD = timeBetweenInvisibility; 
    }

    
    void Update()
    {
        if(timeBetweenInvisibilityCD >= 0)
        {
            timeBetweenInvisibilityCD -= Time.deltaTime;
        }
        else if(timeBeforeHiddingCD >= 0)
        {
            animator.SetTrigger("SignalHidding");
            timeBeforeHiddingCD -= Time.deltaTime;
        }
        else if(durationOfInvisibilityCD >= 0)
        {
            animator.SetTrigger("Invisible");
            durationOfInvisibilityCD -= Time.deltaTime;
            isInvisible = true;
        }
        else
        {
            animator.SetTrigger("Visible");
            durationOfInvisibilityCD = durationOfInvisibility;
            timeBeforeHiddingCD = timeBeforeHidding;
            timeBetweenInvisibilityCD = timeBetweenInvisibility;
            isInvisible = false;
        }
    }
}
