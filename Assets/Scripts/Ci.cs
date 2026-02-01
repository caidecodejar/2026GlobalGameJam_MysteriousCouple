using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ci : MonoBehaviour
{
    public bool isBlack;
    private PolygonCollider2D pc;

    private void Start()
    {
        isBlack = gameObject.GetComponent<InvertableObject>().isBlack;
        pc = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        isBlack = gameObject.GetComponent<InvertableObject>().isBlack;
        if (!isBlack)
        {
            pc.isTrigger = true;
        }
        else
        {
            pc.isTrigger = false;
        }
    }
}
