using UnityEngine;

/// <summary>
/// 大摆锤控制脚本
/// 挂载到摆锤支点空对象，锤头位置通过Inspector面板自定义坐标
/// </summary>
public class PendulumController : MonoBehaviour
{
    [Header("摆锤核心配置")]
    public Vector3 hammerLocalPos; // 锤头相对支点的本地坐标（直接输入自定义坐标）
    public float swingAngle = 60f; // 最大摆动角度（单位：度，越大摆幅越宽）
    public float swingSpeed = 1.5f; // 摆动速度（数值越大摆动越快）
    [Header("摆锤外观（可选，快速可视化）")]
    public float armWidth = 0.2f; // 摆臂宽度
    public float hammerRadius = 0.5f; // 锤头半径
    public Color armColor = Color.gray; // 摆臂颜色
    public Color hammerColor = Color.black; // 锤头颜色

    private LineRenderer armLine; // 摆臂（用LineRenderer实现，无需模型）
    private Transform hammerTrans; // 锤头Transform
    private float currentAngle; // 当前摆动角度

    void Start()
    {
        // 初始化摆臂和锤头（自动创建，无需手动搭建模型）
        InitPendulumVisual();
    }

    void Update()
    {
        // 核心摆动逻辑：正弦曲线实现平滑往复摆动
        currentAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        // 旋转摆锤（绕支点Z轴旋转，2D/3D通用，可根据视角调整旋转轴）
        transform.localEulerAngles = new Vector3(0, 0, currentAngle);
        // 更新摆臂线条（跟随摆动）
        UpdateArmLine();
    }

    /// <summary>
    /// 初始化摆臂和锤头（自动创建GameObject，无需手动搭建）
    /// </summary>
    void InitPendulumVisual()
    {
        // 1. 创建摆臂（LineRenderer，无模型更轻便）
        GameObject armObj = new GameObject("PendulumArm");
        armObj.transform.SetParent(transform, false);
        armLine = armObj.AddComponent<LineRenderer>();
        armLine.positionCount = 2; // 两点组成一条线
        armLine.startWidth = armWidth;
        armLine.endWidth = armWidth;
        armLine.material = new Material(Shader.Find("Unlit/Color"));
        armLine.material.color = armColor;
        armLine.useWorldSpace = false; // 使用本地坐标

        // 2. 创建锤头（球体，可直接修改坐标）
        GameObject hammerObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hammerObj.name = "PendulumHammer";
        hammerObj.transform.SetParent(transform, false);
        hammerTrans = hammerObj.transform;
        // 设置锤头位置（使用自定义的本地坐标）
        hammerTrans.localPosition = hammerLocalPos;
        // 设置锤头大小和颜色
        hammerTrans.localScale = Vector3.one * hammerRadius * 2;
        Renderer hammerRender = hammerObj.GetComponent<Renderer>();
        hammerRender.material = new Material(Shader.Find("Unlit/Color"));
        hammerRender.material.color = hammerColor;

        // 3. 给锤头添加碰撞体（可选，检测玩家碰撞）
        SphereCollider collider = hammerObj.AddComponent<SphereCollider>();
        collider.radius = hammerRadius;
        // 如需检测玩家触发，勾选IsTrigger；如需物理碰撞，保持未勾选
        collider.isTrigger = true;

        // 4. 给锤头添加标签（方便后续检测碰撞）
        hammerObj.tag = "PendulumHammer";
    }

    /// <summary>
    /// 更新摆臂线条位置（始终连接支点和锤头）
    /// </summary>
    void UpdateArmLine()
    {
        armLine.SetPosition(0, Vector3.zero); // 起点：支点位置（本地坐标0,0,0）
        armLine.SetPosition(1, hammerLocalPos); // 终点：锤头本地坐标
    }

    // 可选：锤头碰撞检测（挂载到锤头也可，这里直接集成）
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 检测玩家标签
        {
            // 玩家被击中逻辑，示例：触发受伤/游戏结束
            Debug.Log("玩家被大摆锤击中！");
            // 可在这里调用玩家受伤脚本：other.GetComponent<PlayerController>().TakeDamage();
        }
    }
}