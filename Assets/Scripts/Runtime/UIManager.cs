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
    private Transform m_showedPanelGroup;
    private Transform m_cachedGroup;
    private EventSystem m_eventSystem;

    private List<UIPanel> existPanels;
    private Dictionary<string, CachedPanel> cachedPanels;
    
    private int minorder = 0;
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
            Debug.LogError("There is currently a UIPanel openning in progress, cannot open other UIPanels during this period!");
            return;
        }

        isOpeningPanel = true;
        PreloadPanel(panelName, from, (isSuccess) =>
        {
            if (!isSuccess)
            {
                Debug.LogError($"无法打开UI面板\"{panelName}\"");
                isOpeningPanel = false;
            }

            UIPanel panel = cachedPanels[panelName].Panel;
            cachedPanels.Remove(panelName);

            panel.Canvas.sortingOrder = order;
            panel.gameObject.SetActive(true);
            panel.transform.SetParent(m_showedPanelGroup);
            panel.OnShow();
            
            order++;
            isOpeningPanel = false;
        });
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
            cachedPanels[panelName].IncreaseRefCount();
            callback?.Invoke(true);
            return;
        }

        LoadPanelPrefabAsync(panelName, from, (prefab) =>
        {
            if (prefab == null)
            {
                Debug.LogError($"Can not load UIPanel prefab asset named \"{panelName}\" from {from.ToString()}");
                callback?.Invoke(false);
                return;
            }

            var go = Instantiate(prefab);
            UIPanel panel = go.GetComponent<UIPanel>();

            if (panel == null)
            {
                Destroy(go);
                Debug.LogError($"Loaded asset \"{panelName}\" from {from.ToString()} is not a standard UIPanel prefab asset, please attach a UIPanel Component to it!");
                callback?.Invoke(false);
                return;
            }

            go.SetActive(false);
            go.transform.SetParent(m_cachedGroup);
            CachedPanel cachedPanel = new CachedPanel(panel, panelName);
            cachedPanels.Add(panelName, cachedPanel);
            callback?.Invoke(true);
        });
    }

    /// <summary>
    /// 卸载缓存的UI面板
    /// </summary>
    /// <param name="panelName">要卸载的UI面板名称</param>
    public void UnloadPanel(string panelName)
    {
        if (!cachedPanels.ContainsKey(panelName))
        {
            return;
        }

        cachedPanels[panelName].DecreaseRefCount();
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

        m_showedPanelGroup = new GameObject("Active Panels").transform;
        m_cachedGroup = new GameObject("Cached Panels").transform;
        m_showedPanelGroup.SetParent(transform);
        m_cachedGroup.SetParent(transform);
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
        private UIPanel panel;
        private string key;
        private int refCount = 1;

        #region 构造器
        public CachedPanel(UIPanel panel, string key)
        {
            this.panel = panel;
            this.key = key;
        }
        #endregion

        #region 属性
        public UIPanel Panel { get => panel; }
        public string Key { get => key; }
        #endregion

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

    public class PanelDependency 
    {
        private string key;
        private LoadPanelPrefabFrom loadPanelPrefabFrom;

        public PanelDependency(string key, LoadPanelPrefabFrom loadPanelPrefabFrom)
        {
            this.key = key;
            this.loadPanelPrefabFrom = loadPanelPrefabFrom;
        }

        public string Key { get => key; }
        public LoadPanelPrefabFrom LoadPanelPrefabFrom { get => loadPanelPrefabFrom; }
    }
    #endregion
}