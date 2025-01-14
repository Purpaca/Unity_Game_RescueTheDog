using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Singleton;
using EventSystem = UnityEngine.EventSystems.EventSystem;

/// <summary>
/// UI管理器-负责层级和UI栈的管理
/// </summary>
public class UIManager : AutoInstantiateMonoSingleton<UIManager>
{
    private EventSystem m_eventSystem;

    private List<UIPanel> existPanels;
    private Dictionary<string, CachedPanel> cachedPanels;
    
    private int order = 0;
    private bool isOpeningPanel = false;

    #region 属性
    public static UIManager Instance { get => instance; }
    #endregion

    #region Public 方法
    /// <summary>
    /// 打开一个UI面板至顶层
    /// </summary>
    public void ShowPanel(string panelName, UnityAction callback = null, LoadPanelPrefabFrom from = LoadPanelPrefabFrom.AssetBundle)
    {
        if (isOpeningPanel)
        {
            Debug.Log("当前有一个UI面板正在打开中，暂时无法打开新的UI面板.");
            return;
        }

        isOpeningPanel = true;
        PreloadPanel(panelName, from, (isPanelPreloaded) =>
        {
            if (!isPanelPreloaded)
            {
                Debug.LogError($"无法打开UI面板\"{panelName}\"");
                isOpeningPanel = false;
            }

            cachedPanels[panelName].panel.Canvas.sortingOrder = order;
            
            order++;
            isOpeningPanel = false;
        });
    }

    public void Close(UIPanel panel) 
    {

    }

    /// <summary>
    /// 关闭最顶层的UI面板
    /// </summary>
    public void CloseTopPanel() 
    {

    }

    /// <summary>
    /// 预加载指定的UI面板
    /// </summary>
    /// <param name="panelName">要加载的UI面板预制体的名字</param>
    /// <param name="from">加载UI面板预制体的模式</param>
    /// <param name="callback">当预加载请求结束后的回调方法</param>
    public void PreloadPanel(string panelName, LoadPanelPrefabFrom from = LoadPanelPrefabFrom.AssetBundle, UnityAction<bool> callback = null)
    {
        if (cachedPanels.ContainsKey(panelName))
        {
            callback?.Invoke(true);
            return;
        }

        LoadPanelPrefabAsync(panelName, from, (prefab) =>
        {
            if (prefab == null)
            {
                Debug.LogError($"无法从{from.ToString()}加载名为\"{panelName}\"的UI预制体资源！");
                callback?.Invoke(false);
                return;
            }

            var go = Instantiate(prefab);
            UIPanel panel = go.GetComponent<UIPanel>();

            if (panel == null)
            {
                Destroy(go);
                Debug.LogError($"从{from.ToString()}加载的名为\"{panelName}\"的预制体资源不是一个标准的UIPanel预制体！请为其挂载UIPanel脚本组件。");
                callback?.Invoke(false);
                return;
            }

            go.SetActive(false);
            CachedPanel cachedPanel = new() { key = panelName, panel = panel };
            cachedPanels.Add(panelName, cachedPanel);
            callback?.Invoke(true);
        });
    }
    #endregion

    #region Private 方法
    /// <summary>
    /// 异步加载UI面板预制体资源
    /// </summary>
    /// <param name="panelName">要加载的UI面板预制体的名字</param>
    /// <param name="from">加载UI面板预制体的模式</param>
    /// <param name="callback">加载请求结束后的回调方法，返回值代表 加载是否成功？</param>
    private void LoadPanelPrefabAsync(string panelName, LoadPanelPrefabFrom from, UnityAction<GameObject> callback)
    {
        switch (from)
        {
            case LoadPanelPrefabFrom.Resources:
                var request = Resources.LoadAsync<GameObject>($"Prefabs/UI/{panelName}");
                request.completed += (operation) =>
                {
                    callback?.Invoke(request.asset as GameObject);
                };
                break;

            case LoadPanelPrefabFrom.AssetBundle:
                AssetManager.LoadUIPrefabAsync(panelName, (prefab) =>
                {
                    callback?.Invoke(prefab);
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

        cachedPanels = new();

        if (EventSystem.current != null)
        {
            m_eventSystem = EventSystem.current;
            EventSystem.current.transform.SetParent(transform, false);
        }
        else
        {
            m_eventSystem = Instantiate(Resources.Load<GameObject>("Prefabs/EventSystem")).GetComponent<EventSystem>();
            m_eventSystem.transform.SetParent(transform, false);
        }
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

    private class CachedPanel
    {
        public string key;
        public UIPanel panel;
        private int refCount = 1;

        public void IncreaseRefCount() 
        {
            refCount++;
        }
        
        public void DecreaseRefCount() 
        {
            refCount--;
            if(refCount <= 0) 
            {
                Destroy(panel.gameObject);
                Instance.cachedPanels.Remove(key);
            }
        }
    }
    #endregion
}