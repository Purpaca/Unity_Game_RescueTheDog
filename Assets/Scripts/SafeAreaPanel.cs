using UnityEngine;
using UnityEngine.UI;
using UIManagement;

public class SafeAreaPanel : Panel
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
    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();
        m_sliderUp.onValueChanged.AddListener((value) =>
        {
            Vector2 offsetMax = new Vector2(m_marker.offsetMax.x, rectTransform.rect.height / 2 * value);
            m_marker.offsetMax = offsetMax;
            UserSettings.Instance.SafeAreaOffsetUp = value;
        });

        m_sliderDown.onValueChanged.AddListener((value) =>
        {
            Vector2 offsetMin = new Vector2(m_marker.offsetMin.x, -rectTransform.rect.height / 2 * value);
            m_marker.offsetMin = offsetMin;
            UserSettings.Instance.SafeAreaOffsetDown = value;
        });

        m_sliderLeft.onValueChanged.AddListener((value) =>
        {
            Vector2 offsetMin = new Vector2(-rectTransform.rect.width / 2 * value, m_marker.offsetMin.y);
            m_marker.offsetMin = offsetMin;
            UserSettings.Instance.SafeAreaOffsetLeft = value;
        });

        m_sliderRight.onValueChanged.AddListener((value) =>
        {
            Vector2 offsetMax = new Vector2(rectTransform.rect.width / 2 * value, m_marker.offsetMax.y);
            m_marker.offsetMax = offsetMax;
            UserSettings.Instance.SafeAreaOffsetRight = value;
        });
    }

    private void OnEnable()
    {
        m_sliderUp.value = UserSettings.Instance.SafeAreaOffsetUp;
        m_sliderDown.value = UserSettings.Instance.SafeAreaOffsetDown;
        m_sliderLeft.value = UserSettings.Instance.SafeAreaOffsetLeft;
        m_sliderRight.value = UserSettings.Instance.SafeAreaOffsetRight;
    }
    #endregion
}