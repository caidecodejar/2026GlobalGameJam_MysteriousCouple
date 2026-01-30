using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 精简版：按钮按名称切换场景+悬停/点击音效+退出游戏
/// 自动悬浮音效 | 按场景名切换 | 退出游戏 | 音效不中断
/// </summary>
public class SceneSwitcherSimple : MonoBehaviour, IPointerEnterHandler
{
    [Header("功能开关")]
    public bool isQuitButton; // 勾选=退出按钮 | 不勾选=按名称切场景

    [Header("场景配置")]
    public string targetSceneName; // 填写Build Settings中的场景准确名称（区分大小写）

    [Header("音效配置")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    [Range(0f, 1f)] public float soundVolume = 0.8f;
    public float delay = 0.2f; // 音效播放延迟，确保完整播放

    private AudioSource _audio;
    private Button _btn;

    void Awake()
    {
        // 自动创建/获取AudioSource，解决MissingComponentException报错
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
        {
            _audio = gameObject.AddComponent<AudioSource>();
        }
        // 2D音效配置，不受3D场景位置影响
        _audio.volume = soundVolume;
        _audio.playOnAwake = false;
        _audio.spatialBlend = 0f;

        // 初始化按钮并绑定点击事件
        _btn = GetComponent<Button>();
        if (_btn != null)
        {
            _btn.onClick.AddListener(() =>
            {
                // 播放点击音效（双重判空，避免报错）
                if (clickSound != null && _audio != null)
                    _audio.PlayOneShot(clickSound);
                // 根据功能开关执行对应逻辑：退出游戏 / 按名称切场景
                Invoke(isQuitButton ? nameof(QuitGame) : nameof(SwitchSceneByName), delay);
            });
        }
    }

    // 鼠标悬浮自动播放音效（实现接口，无需手动配置EventTrigger）
    public void OnPointerEnter(PointerEventData e)
    {
        if (hoverSound != null && _audio != null)
            _audio.PlayOneShot(hoverSound);
    }

    /// <summary>
    /// 核心修改：按场景名称切换（替代原索引+1）
    /// 需填写targetSceneName，匹配Build Settings中的准确名称
    /// </summary>
    void SwitchSceneByName()
    {
        // 判空保护，避免未填写场景名导致报错
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("请在Inspector面板填写目标场景名称（Build Settings中的准确名称）！");
            return;
        }
        // 加载指定名称的场景
        SceneManager.LoadScene(targetSceneName);
    }

    // 退出游戏（适配编辑器/真机，原有功能不变）
    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}