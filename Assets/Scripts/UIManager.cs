using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Singleton;
using UIManagement;
using EventSystem = UnityEngine.EventSystems.EventSystem;

/// <summary>
/// UI管理器-负责层级和UI栈的管理
/// </summary>
public class UIManager : AutomaticSingletonMonoBehaviour<UIManager>
{
    private List<Panel> existPanels;
    private Dictionary<string, GameObject> loadedPanelPrefab;

    private int order = 0;
    private EventSystem _eventSystem;

    #region 属性
    public static UIManager Instance { get => instance; }
    #endregion

    #region Public 方法
    /// <summary>
    /// 创建一个空白的Canvas
    /// </summary>
    /// <returns></returns>
    public Canvas GetEmptyCanvas() 
    {
        var asset = Resources.Load<GameObject>("Prefabs/Canvas");
        
        Canvas canvas = Instantiate(asset).GetComponent<Canvas>();
        canvas.sortingOrder = order;
        order++;
        return canvas;
    }

    public void CreatePanel(string panelName, LoadPanelPrefabFrom from = LoadPanelPrefabFrom.AssetBundle) 
    {
        if(LoadPanelPrefab(panelName, from))
        {
            GameObject panelObj = Instantiate(loadedPanelPrefab[panelName]);
            Panel panel = panelObj.GetComponent<Panel>();

            if(panel == null)
            {
                Debug.LogError($"");
                Destroy(panelObj);
                return;
            }
        }
    }

    /// <summary>
    /// 加载UI面板预制体资源
    /// </summary>
    /// <param name="panelName">要加载的UI面板预制体的名字</param>
    /// <param name="from">加载UI面板预制体的模式</param>
    /// <returns>加载是否成功？</returns>
    public bool LoadPanelPrefab(string panelName, LoadPanelPrefabFrom from)
    {
        if (!loadedPanelPrefab.ContainsKey(panelName))
        {
            GameObject asset = null;
            switch (from)
            {
                case LoadPanelPrefabFrom.Resources:
                    asset = Resources.Load<GameObject>($"Prefabs/UI/{panelName}");
                    break;
                case LoadPanelPrefabFrom.AssetBundle:
                    asset = AssetManager.LoadUIPrefab(panelName);
                    break;
            }

            bool isSucceed = !(asset == null);
            if (isSucceed)
            {
                loadedPanelPrefab.Add(panelName, asset);
            }
            return isSucceed;
        }
        return true;
    }

    /// <summary>
    /// 异步加载UI面板预制体资源
    /// </summary>
    /// <param name="panelName">要加载的UI面板预制体的名字</param>
    /// <param name="from">加载UI面板预制体的模式</param>
    /// <param name="callback">加载请求结束后的回调方法，返回值代表 加载是否成功？</param>
    public void LoadPanelPrefabAsync(string panelName, LoadPanelPrefabFrom from, UnityAction<bool> callback = null)
    {
        if (loadedPanelPrefab.ContainsKey(panelName))
        {
            callback?.Invoke(true);
            return;
        }

        switch (from)
        {
            case LoadPanelPrefabFrom.Resources:
                var request = Resources.LoadAsync<GameObject>($"Prefabs/UI/{panelName}");
                request.completed += (operation) =>
                {
                    bool isSucceed = !(request.asset == null);
                    if (isSucceed)
                    {
                        loadedPanelPrefab.Add(panelName, request.asset as GameObject);
                    }
                    callback?.Invoke(isSucceed);
                };
                break;

            case LoadPanelPrefabFrom.AssetBundle:
                AssetManager.LoadUIPrefabAsync(panelName, (prefab) =>
                {
                    bool isSucceed = !(prefab == null);
                    if (isSucceed)
                    {
                        loadedPanelPrefab.Add(panelName, prefab);
                    }
                    callback?.Invoke(isSucceed);
                });
                break;
        }
    }
    #endregion

    #region Unity 消息
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        loadedPanelPrefab = new Dictionary<string, GameObject>();

        _eventSystem = Instantiate(Resources.Load<GameObject>("Prefabs/EventSystem")).GetComponent<EventSystem>();
        _eventSystem.transform.SetParent(transform, false);
    }
    #endregion

    #region 内部类型
    /// <summary>
    /// UI面板预制体资源加载的规则
    /// </summary>
    public enum LoadPanelPrefabFrom
    {
        /// <summary>
        /// 从Resources中加载
        /// </summary>
        Resources,

        /// <summary>
        /// 从AssetBundle中加载
        /// </summary>
        AssetBundle
    }
    #endregion
}