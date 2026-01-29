using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 要切换到的游戏场景名称（替换为你当前游戏场景的名称，比如"test"）
    public string gameSceneName = "test";

    // 按钮点击时调用的方法
    public void LoadGameScene()
    {
        // 加载游戏场景
        SceneManager.LoadScene(gameSceneName);
    }
}
