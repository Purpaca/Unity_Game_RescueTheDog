using UnityEngine;
using Singleton;
using UnityEngine.EventSystems;
using UIManagement;

/// <summary>
/// UI管理器-负责层级和UI栈的管理
/// </summary>
public class UIManager : AutomaticSingletonMonoBehaviour<UIManager>
{
    private int orderIndex = 0;
    private EventSystem _eventSystem;

    #region 属性
    public static UIManager Instance { get => instance; }
    #endregion

    #region Public 方法
    public void Add(Panel panel) 
    {
        panel.canvas.sortingOrder = orderIndex;
        orderIndex++;
    }
    #endregion

    #region Unity 消息
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        _eventSystem = Instantiate(Resources.Load<GameObject>("Prefabs/EventSystem")).GetComponent<EventSystem>();
        _eventSystem.transform.SetParent(transform, false);
    }
    #endregion
}