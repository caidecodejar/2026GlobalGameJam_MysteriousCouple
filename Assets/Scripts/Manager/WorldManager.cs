using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldManager : SingletonMonoBase<WorldManager>
{
    private WorldManager(){}
    
    
    public int currentLevel = 1;

    // 每一关对应一个 limit，在 Inspector 里配置
    public List<int> levelLimits = new List<int>();

    // 第一关的 limit，默认值为 2
    public int limit = 2;

    public ArrayList levels = new ArrayList();
    
    private void Start()
    {
        limit = levelLimits[currentLevel];
        DontDestroyOnLoad(gameObject);
        EventManager.AddListener(EventType.NextLevel,LoadNextLevel);
    }

    private void LoadNextLevel()
    {
        SetLevel(currentLevel + 1);
        currentLevel++;
    }

   

    // 设置当前关卡，并根据关卡更新 limit
    public void SetLevel(int levelIndex)
    {
        // 防止越界
        //currentLevel = Mathf.Clamp(levelIndex, 0, Mathf.Max(levelLimits.Count - 1, 0));

        if (levelLimits.Count > 0)
        {
            limit = levelLimits[currentLevel+1];
        }
        else
        {
            // 如果没配置，使用一个默认值
            limit = 2;
        }

        // TODO: 在这里加载关卡数据、刷新场景等

        if (currentLevel + 1 == 6)
        {
            SceneMgr.Instance.LoadScene("EndMenu", null);
            return;
        }
        
        SceneMgr.Instance.LoadScene("Scene" + (currentLevel + 1), null);
        Debug.Log($"切换到关卡 {currentLevel + 1}，limit = {limit}");
    }


    private void OnDisable()
    {
        EventManager.RemoveListener(EventType.NextLevel,LoadNextLevel);
    }
}
