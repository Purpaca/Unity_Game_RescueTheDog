using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public abstract class UIPanel : MonoBehaviour
{
    private Canvas m_canvas;
    private CanvasGroup m_canvasGroup;

    private bool isFrozen;

    #region 属性
    public Canvas Canvas
    {
        get
        {
            if (m_canvas == null)
            {
                m_canvas = GetComponent<Canvas>();
            }

            return m_canvas;
        }
    }

    public CanvasGroup CanvasGroup 
    {
        get
        {
            if (m_canvasGroup == null)
            {
                m_canvasGroup = GetComponent<CanvasGroup>();
            }

            return m_canvasGroup;
        }
    }

    /// <summary>
    /// 需要预加载的UI面板
    /// </summary>
    public virtual string[] PreloadPanels { get => null; }
    #endregion

    #region Public 方法
    /// <summary>
    /// 冻结此面板
    /// </summary>
    public void Freeze()
    {
        if (!isFrozen)
        {
            CanvasGroup.interactable = false;
            
            OnFreeze();
            isFrozen = true;
        }
    }

    /// <summary>
    /// 解冻此面板
    /// </summary>
    public void Unfreeze() 
    {
        if (isFrozen) 
        {
            CanvasGroup.interactable = true;

            OnUnfreeze();
            isFrozen = false;
        }
    }

    public virtual void OnShow() { }
    public virtual void OnClose() { }
    public virtual void OnFreeze() { }
    public virtual void OnUnfreeze() { }
    #endregion

    #region Unity 消息
    protected virtual void Awake()
    {
    }

    private void OnDestroy()
    {
        //UIManager.Instance.
    }
    #endregion
}