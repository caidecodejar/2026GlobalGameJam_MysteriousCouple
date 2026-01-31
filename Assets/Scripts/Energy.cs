using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public int type; // 1代表Square，2代表Circle

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Black"))
        {
            Destroy(gameObject);
            
            // 场景2、3拾取能量后要显示UI更新
            EventManager.Broadcast(EventType.Scene2EnergyCollect);
            
            //后续场景UI通用
            EventManager.Broadcast(EventType.EnergyCollect, type);
        }
        
    }
}
