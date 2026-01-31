using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : SingletonMonoBase<BuildManager>
{
    
    public int senery = 0;//Square能量数
    public int cenergy = 0;//Circle能量数

    public int privateSenergy = 0;
    public int privateCenergy = 0;
    
    // 要生成的方块预制体
    public GameObject blockPrefab;
    public GameObject ghostPrefab;
    public GameObject ghostInstance;    // 当前的虚影对象
    private Camera mainCam;

    void Start()
    {
        //blockPrefab = Resources.Load<GameObject>("Prefabs/SquareCreate");
        //ghostPrefab = Resources.Load<GameObject>("Prefabs/temSquareCreate");
        
        mainCam = Camera.main;
        DontDestroyOnLoad(gameObject);
        
        //只针对Scene3
        EventManager.AddListener(EventType.CancelFixed, () => { cenergy=1;});
        EventManager.AddListener<int>(EventType.EnergyCollect, EnergyCollect);
    }

    private void EnergyCollect(int type)
    {
        if (type == 1)
        {
            senery++;
            privateSenergy++;
        }

        if (type == 2)
        {
            cenergy++;
            privateCenergy++;
        }
        EventManager.Broadcast(EventType.UpdateAllUI);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventType.CancelFixed, () => { cenergy=1;});
        EventManager.RemoveListener<int>(EventType.EnergyCollect, EnergyCollect);
    }

    void Update()
    {
        HandleStartGhost();
        HandleUpdateGhost();
        HandlePlaceOrCancel();
    }

    // 按下 E 开始预览
    void HandleStartGhost()
    {
        
        
        if (Input.GetKeyDown(KeyCode.E) && senery>0)
        {
            ghostInstance = Instantiate(ghostPrefab);

            if (!ghostPrefab)
            {
                Debug.LogError("BuildManager: ghostPrefab 未赋值");
                return;
            }
            
            //ghostInstance = Object.Instantiate(ghostPrefab);
            Debug.Log("生成虚影: " + ghostInstance.name);
            
            // 禁用不需要的组件（如碰撞、脚本），避免干扰
            foreach (var col in ghostInstance.GetComponentsInChildren<Collider2D>())
                col.enabled = false;
            
        }
    }

    // 虚影跟随鼠标
    void HandleUpdateGhost()
    {
        if (!ghostInstance) return;
        
        if (!mainCam)
            mainCam = Camera.main;

        if (!mainCam)
            return;
        
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCam.transform.position.z); // 2D 正交可随便给个正值
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        // // 如果需要网格对齐，可用 Mathf.Round
        // worldPos.x = Mathf.Round(worldPos.x);
        // worldPos.y = Mathf.Round(worldPos.y);
        // worldPos.z = 0f;

        ghostInstance.transform.position = worldPos;
    }

    // 鼠标点击生成，或取消
    void HandlePlaceOrCancel()
    {
        if (!ghostInstance) return;

        // 左键生成实体方块
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 placePos = ghostInstance.transform.position;
            Instantiate(blockPrefab, placePos, Quaternion.identity);
            Destroy(ghostInstance);
            senery--;
            EventManager.Broadcast(EventType.UpdateSenergyUI);
            
        }

        // 右键或 再按一次E 取消
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(ghostInstance);
        }
    }
}
