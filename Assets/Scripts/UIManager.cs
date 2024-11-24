using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Singleton;
using UIManagement;

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
        
        Resources.UnloadAsset(asset);
        
        return canvas;
    }

    public void CreatePanel(string panelName, LoadPanelPrefabFrom from = LoadPanelPrefabFrom.AssetBundle) 
    {
        if(LoadPanelPrefab(panelName, from))
        {
            var panel = Instantiate(loadedPanelPrefab[panelName]).GetComponent<Panel>();
        }
    }

    /// <summary>
    /// 加载UIPanel的预制体资源
    /// </summary>
    /// <param name="assetName">要加载的UIPanel预制体资源的名字</param>
    /// <param name="from">加载UIPanel预制体资源的方式</param>
    /// <returns>是否成功加载UIPanel的预制体资源？</returns>
    public bool LoadPanelPrefab(string assetName, LoadPanelPrefabFrom from)
    {
        if (!loadedPanelPrefab.ContainsKey(assetName))
        {
            GameObject asset = null;
            switch (from)
            {
                case LoadPanelPrefabFrom.Resources:
                    asset = Resources.Load<GameObject>(assetName);
                    break;
                case LoadPanelPrefabFrom.AssetBundle:
                    asset = AssetManager.Instance.LoadUIPrefab(assetName);
                    break;
            }

            if (asset != null)
            {
                loadedPanelPrefab.Add(assetName, asset);
                return true;
            }
            else
            {
                Debug.LogError($"Unable to load UI Panel prefab asset named as \"{assetName}\", from {from.ToString()}.");
                return false;
            }
        }

        return true;
    }

    public void PreloadPanelPrefab(string assetName, LoadPanelPrefabFrom from)
    {
        if (!loadedPanelPrefab.ContainsKey(assetName))
        {
            AssetManager.Instance.LoadUIPrefabAsync(assetName, (prefab) =>
            {
                loadedPanelPrefab.Add(assetName, prefab);
            });
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