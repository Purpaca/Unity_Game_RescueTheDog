using System.IO;
using UnityEngine;
using Newtonsoft.Json;

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

    System.Collections.IEnumerator Initialization()
    {
        //创建开屏动画的GameObject并播放
        SplashController splash = Instantiate(Resources.Load<GameObject>("Prefabs/Splash")).GetComponent<SplashController>();

        #region 游戏应用初始化
        Utils.GetMainCamera();
        GameManager manager = Instantiate(Resources.Load<GameObject>("Prefabs/Game Manager")).GetComponent<GameManager>();

        #region 读取用户的应用设置
        try
        {
            if (File.Exists(UserSettings.FileStoragePath))
            {
                UserSettings.SetInstance(JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(UserSettings.FileStoragePath)));
            }
        }
        catch (System.Exception exception)
        {
            Debug.LogException(exception);
        }

        Application.quitting += () =>
        {
            File.WriteAllText(UserSettings.FileStoragePath, JsonConvert.SerializeObject(UserSettings.Instance));
        };
        #endregion
        
        //创建资源检查器


        #endregion

        while (!splash.IsAnimationDone)
        {
            yield return null;
        }

        Debug.Log(AssetManager.GetInstance());
        Instantiate(AssetManager.GetInstance().LoadUIPrefab("Panel_SafeArea"));

        Destroy(splash.gameObject);
        Destroy(gameObject);
    }
}