using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedSquare : MonoBehaviour
{
    public float lifeTime = 3f;
    public bool isBlack = true;
    public bool isStart = true;
    public bool isFixed = false;
    private SpriteRenderer sr;
    
    private Coroutine lifeCoroutine;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        isBlack = gameObject.GetComponent<InvertableObject>().isBlack;
        isFixed = gameObject.GetComponent<InvertableObject>().isFixed;
    }

    private void Start()
    {
        EventManager.AddListener(EventType.ResetAllUI, OnReset);
    }

    private void OnReset()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventType.ResetAllUI, OnReset);
    }

    private void Update()
    {
        isBlack = gameObject.GetComponent<InvertableObject>().isBlack;
        isFixed = gameObject.GetComponent<InvertableObject>().isFixed;
        if (isBlack && isStart)
        {
            isStart = false;
            StartLifeTimer();
        }
    }

    // void OnEnable()
    // {
    //     // 启用时根据当前颜色决定是否开始计时
    //     StartLifeTimer();
    // }
    //

    private void StartLifeTimer()
    {
        // 确保不重复开启
        StopLifeTimer();
        lifeCoroutine = StartCoroutine(DisappearAfterTime());
    }

    private void StopLifeTimer()
    {
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }
    }

    private IEnumerator DisappearAfterTime()
    {
        float timer = 0f;
        
        // 初始 alpha
        float startAlpha = sr ? sr.color.a : 1f;
        
        while (timer < lifeTime)
        {
            // 过程中如果变成白色，立刻退出协程，不销毁
            if (!isBlack)
            {
                isStart = true;
                // 变白时立刻恢复为不透明，并停止淡出
                if (sr)
                {
                    Color c = sr.color;
                    c.a = 1f;
                    sr.color = c;
                }
                yield break;
            }

            if (isFixed)
            {
                isStart = true;
                // 被固定时立刻变为不透明和灰色，并停止淡出
                if (sr)
                {
                    Color c = Color.gray;
                    c.a = 1f;
                    sr.color = c;
                }
                yield break;
            }

            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / lifeTime);
            if (sr)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(startAlpha, 0f, t); // 从 startAlpha 到 0
                sr.color = c;
            }
            
            yield return null;
        }

        // 计时结束且依然是黑色才销毁
        if (isBlack)
        {
            Destroy(gameObject);
            BuildManager.Instance.senery++;
            EventManager.Broadcast(EventType.UpdateSenergyUI);
        }
    }
    
}
