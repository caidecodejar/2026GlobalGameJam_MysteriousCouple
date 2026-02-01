using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeManager : MonoBehaviour
{
    public Color targetColor = Color.gray;   // 要变成的颜色
    //private bool selectMode = false;        // 是否在选取模式
    private Camera mainCam;

    //public GameObject circlePrefab;
    public GameObject ghostPrefab;
    private GameObject ghostInstance;    // 当前的虚影对象 
    
    // 只检测这一层上的对象（在 Inspector 里把可点击物体设到 Clickable 层）
    public LayerMask clickableLayer;
    
    
    private void Start()
    {
        mainCam = Camera.main;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleStartGhost();
        HandleUpdateGhost();
        HandlePlaceOrCancel();

    }
    
    
    // 按下 Q 开始预览
    void HandleStartGhost()
    {
        if (Input.GetKeyDown(KeyCode.Q) && BuildManager.Instance.cenergy>0)
        {
            if (ghostInstance) return;

            ghostInstance = Instantiate(ghostPrefab);
            // 禁用不需要的组件（如碰撞、脚本），避免干扰
            foreach (var col in ghostInstance.GetComponentsInChildren<Collider2D>())
                col.enabled = false;
        }
    }
    
    // 虚影跟随鼠标
    void HandleUpdateGhost()
    {
        if (!ghostInstance) return;
        
        if(!mainCam)
            mainCam = Camera.main;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCam.transform.position.z); // 2D 正交可随便给个正值
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        // // 如果需要网格对齐，可用 Mathf.Round
        // worldPos.x = Mathf.Round(worldPos.x);
        // worldPos.y = Mathf.Round(worldPos.y);
        // worldPos.z = 0f;

        ghostInstance.transform.position = worldPos;
    }
    
    // 鼠标点击变换颜色，或取消
    void HandlePlaceOrCancel()
    {
        if (!ghostInstance) return;

        // 鼠标左键点击选取一个物体
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log($"[ChangeManager] 点击触发，实例: {GetInstanceID()}，点击前能量: {BuildManager.Instance.cenergy}");
            
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
            Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

            // 2D 射线
            RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero);
            Collider2D targetCol = hit.collider;

            // 若没有碰撞体，则用 OverlapPoint 在该点找一个对象
            if (!targetCol)
            {
                targetCol = Physics2D.OverlapPoint(worldPos2D, clickableLayer);
            }
            
            
            if (targetCol)
            {
                // 尝试获取 SpriteRenderer 并改颜色
                SpriteRenderer sr = targetCol.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.color = targetColor;
                    BuildManager.Instance.cenergy--;
                    //Debug.Log($"[ChangeManager] 实例: {GetInstanceID()} 扣完后能量: {BuildManager.Instance.cenergy}");
                    EventManager.Broadcast(EventType.UpdateAllUI);
                }
                InvertableObject inv = targetCol.GetComponent<InvertableObject>();
                if (inv)
                    inv.isFixed = true;
                EventManager.Broadcast(EventType.Fixed);
                Destroy(ghostInstance);
            }
        }

        // 右键取消
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(ghostInstance);
        }
    }
}
