using UnityEngine;
using UIManagement;

/// <summary>
/// 受屏幕安全区影响的UI面板
/// </summary>
public abstract class SafeAreaAdaptivePanel : Panel
{
    [SerializeField]
    private RectTransform m_adaptiveRect;

    /// <summary>
    /// 使目标矩形适配安全区
    /// </summary>
    public void AdaptToSafeArea()
    {
        Vector2 offsetMin = new Vector2();
        offsetMin.x = -canvas.GetComponent<RectTransform>().rect.width / 2 * UserSettings.Instance.SafeAreaOffsetLeft;
        offsetMin.y = -canvas.GetComponent<RectTransform>().rect.height / 2 * UserSettings.Instance.SafeAreaOffsetDown;

        Vector2 offsetMax = new Vector2();
        offsetMax.x = canvas.GetComponent<RectTransform>().rect.width / 2 * UserSettings.Instance.SafeAreaOffsetRight;
        offsetMax.y = canvas.GetComponent<RectTransform>().rect.height / 2 * UserSettings.Instance.SafeAreaOffsetUp;

        m_adaptiveRect.offsetMin = offsetMin;
        m_adaptiveRect.offsetMax = offsetMax;
    }

    protected override void Awake() 
    {
        base.Awake();

        m_adaptiveRect.pivot = new Vector2(0.5f, 0.5f);
        m_adaptiveRect.anchorMin = new Vector2(0.5f, 0.5f);
        m_adaptiveRect.anchorMax = new Vector2(0.5f, 0.5f);
        m_adaptiveRect.localPosition = Vector3.zero;
    }

    protected virtual void OnEnable()
    {
        AdaptToSafeArea();
    }
}