using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 屏幕安全区设置UI面板
/// </summary>
public class SafeAreaSettingPanel : UIPanel
{
    [SerializeField]
    private RectTransform m_marker;
    [Space, SerializeField]
    private Slider m_sliderUp;
    [SerializeField]
    private Slider m_sliderDown;
    [SerializeField]
    private Slider m_sliderLeft;
    [SerializeField]
    private Slider m_sliderRight;
    [Space, SerializeField]
    Button m_buttonConfirm;

    #region Unity 消息
    protected override void OnInit()
    {
        var rectTransform = GetComponent<RectTransform>();
        m_sliderUp.onValueChanged.AddListener((value) =>
        {
            Vector2 offsetMax = new Vector2(m_marker.offsetMax.x, rectTransform.rect.height / 2 * value);
            m_marker.offsetMax = offsetMax;
            GameManager.Instance.UserSettingsData.SafeAreaOffsetUp = value;
        });

        m_sliderDown.onValueChanged.AddListener((value) =>
        {
            Vector2 offsetMin = new Vector2(m_marker.offsetMin.x, -rectTransform.rect.height / 2 * value);
            m_marker.offsetMin = offsetMin;
            GameManager.Instance.UserSettingsData.SafeAreaOffsetDown = value;
        });

        m_sliderLeft.onValueChanged.AddListener((value) =>
        {
            Vector2 offsetMin = new Vector2(-rectTransform.rect.width / 2 * value, m_marker.offsetMin.y);
            m_marker.offsetMin = offsetMin;
            GameManager.Instance.UserSettingsData.SafeAreaOffsetLeft = value;
        });

        m_sliderRight.onValueChanged.AddListener((value) =>
        {
            Vector2 offsetMax = new Vector2(rectTransform.rect.width / 2 * value, m_marker.offsetMax.y);
            m_marker.offsetMax = offsetMax;
            GameManager.Instance.UserSettingsData.SafeAreaOffsetRight = value;
        });
    }

    private void OnEnable()
    {
        
        m_sliderUp.value = GameManager.Instance.UserSettingsData.SafeAreaOffsetUp;
        m_sliderDown.value = GameManager.Instance.UserSettingsData.SafeAreaOffsetDown;
        m_sliderLeft.value = GameManager.Instance.UserSettingsData.SafeAreaOffsetLeft;
        m_sliderRight.value = GameManager.Instance.UserSettingsData.SafeAreaOffsetRight;
        
    }
    #endregion
}