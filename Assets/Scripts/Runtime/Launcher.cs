using UnityEngine;
using Newtonsoft.Json;
using IEnumerator = System.Collections.IEnumerator;

/// <summary>
/// 整个游戏应用程序的入口
/// </summary>
public sealed class Launcher : MonoBehaviour
{
    private static bool isEverLaunched = false;

    #region Unity 消息
    private void Awake()
    {
        if (isEverLaunched)
        {
            Destroy(gameObject);
            return;
        }

        isEverLaunched = true;
        StartCoroutine(Initialization());
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
    #endregion

    IEnumerator Initialization()
    {
        // 创建开屏动画的GameObject并播放
        SplashController splash = Instantiate(Resources.Load<GameObject>("Prefabs/Splash")).GetComponent<SplashController>();

        #region 游戏应用初始化
        Utils.GetMainCamera();
        GameManager manager = Instantiate(Resources.Load<GameObject>("Prefabs/Game Manager")).GetComponent<GameManager>();

        // TODO:
        // 1.资源检查和远程获取相关
        // 2.创建游戏主界面
        #endregion

        while (!splash.IsAnimationDone)
        {
            yield return null;
        }

        Destroy(splash.gameObject);
        Destroy(gameObject);
    }
}