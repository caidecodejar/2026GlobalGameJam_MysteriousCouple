using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 引入UI命名空间，用于按钮事件

/// <summary>
/// 按钮切换场景 + 点击/接触（悬停）音效
/// 支持按索引切换下一个场景、指定名称/索引切换任意场景
/// </summary>
public class SceneSwitcherWithSound : MonoBehaviour
{
    [Header("===== 场景切换配置 =====")]
    [Tooltip("按名称切换时，填写Build Settings中的场景准确名称（区分大小写）")]
    public string targetSceneName;
    [Tooltip("按索引切换时，填写Build Settings中的场景索引（从0开始）")]
    public int targetSceneIndex;

    [Header("===== 音效配置 =====")]
    [Tooltip("鼠标接触（悬停）按钮时的音效")]
    public AudioClip hoverSound; // 拖拽音效文件到该槽位
    [Tooltip("鼠标点击按钮时的音效")]
    public AudioClip clickSound; // 拖拽音效文件到该槽位
    [Tooltip("音效音量（0-1）")]
    [Range(0f, 1f)] public float soundVolume = 0.8f;

    private AudioSource audioSource; // 音频源组件，用于播放音效
    private Button targetButton;     // 绑定的目标按钮

    void Awake()
    {
        // 获取/创建音频源组件（无需手动添加，代码自动处理）
        // 音频源与脚本挂载在同一对象，不影响其他音效播放
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // 配置音频源基础参数
        audioSource.volume = soundVolume;
        audioSource.playOnAwake = false; // 禁止运行时自动播放音效

        // 获取当前对象上的Button组件（若脚本挂载在按钮上，直接获取）
        targetButton = GetComponent<Button>();
        if (targetButton != null)
        {
            // 绑定按钮点击事件（核心：点击切换下一个场景）
            targetButton.onClick.AddListener(() =>
            {
                PlayClickSound(); // 先播放点击音效，再切换场景
                SwitchToNextScene();
            });
        }
    }

    /// <summary>
    /// 鼠标接触（悬停）按钮时调用（需手动绑定到按钮EventTrigger）
    /// </summary>
    public void PlayHoverSound()
    {
        // 判空：避免未配置音效导致报错
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound); // 播放单次音效，不打断其他音效
        }
    }

    /// <summary>
    /// 播放点击音效（内部调用+可外部绑定）
    /// </summary>
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    /// <summary>
    /// 核心功能：切换到下一个场景（按Build Settings索引自动+1）
    /// 适合顺序场景切换（如场景1→场景2→场景3）
    /// </summary>
    public void SwitchToNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // 判读是否为最后一个场景，若是则跳回第一个场景（可删除该判断，改为不切换）
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextIndex = 0;
            Debug.LogWarning("已到最后一个场景，自动跳回第一个场景！");
        }

        SceneManager.LoadScene(nextIndex);
    }

    /// <summary>
    /// 按场景名称切换（灵活，不受索引顺序影响）
    /// </summary>
    public void SwitchSceneByName()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("请在Inspector面板填写目标场景名称！");
            return;
        }
        SceneManager.LoadScene(targetSceneName);
    }

    /// <summary>
    /// 按场景索引切换（适合固定顺序场景）
    /// </summary>
    public void SwitchSceneByIndex()
    {
        if (targetSceneIndex < 0 || targetSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("场景索引超出范围！请检查Build Settings中的场景列表！");
            return;
        }
        SceneManager.LoadScene(targetSceneIndex);
    }
}