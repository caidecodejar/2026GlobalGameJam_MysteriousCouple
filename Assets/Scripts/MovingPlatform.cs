using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public Transform[] movePos;
    public float waitTime;
    private float originalWaitTime;

    private int i;
    
    void Start()
    {
        i = 0;
        originalWaitTime = waitTime;
    }


    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, movePos[i].position) < 0.1f)
        {
            if (waitTime < 0f)
            {
                i = 1 - i;
                waitTime = originalWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
