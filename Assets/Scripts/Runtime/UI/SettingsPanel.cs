using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 设置面板
/// </summary>
public class SettingsPanel : UIPanel
{
    [SerializeField]
    private Text m_text_version;
    [SerializeField]
    private Slider m_slider_mus, m_slider_sfx;
    [SerializeField]
    private Button m_button_safeArea;
    [SerializeField]
    private Button m_button_btv;
    [SerializeField]
    private Button m_button_github;
    [SerializeField]
    private Button m_button_redX;
    [Space, SerializeField]
    private RectTransform m_body;

    string biliUrl = "https://space.bilibili.com/3546697445673471";
    string githubUrl = "https://github.com/Purpaca/Unity_Game_RescueTheDog";

    #region Public 方法
    public override void OnShow()
    {
        StartCoroutine(OnShowAnimation());
    }
    #endregion

    /// <summary>
    /// UI面板打开动画
    /// </summary>
    IEnumerator OnShowAnimation()
    {
        float axis = 0.5f;
        Vector3 scale = new Vector3(axis, axis, m_body.localScale.z);
        m_body.localScale = scale;

        while (axis < 1.0f)
        {
            axis += 2.0f * Time.deltaTime;
            scale = new Vector3(axis, axis, scale.z);
            m_body.localScale = scale;
            yield return null;
        }

        Debug.Log("............");
    }

    #region Unity 消息
    protected override void OnInit()
    {
        m_slider_mus.onValueChanged.AddListener((value) =>
        {
            GameManager.MusicVolume = value;
            GameManager.Instance.UserSettingsData.MusicVolume = value;
        });

        m_slider_sfx.onValueChanged.AddListener((value) =>
        {
            GameManager.SoundVolume = value;
            GameManager.Instance.UserSettingsData.SfxVolume = value;
        });

        m_button_safeArea.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel("SafeAreaSettingPanel");
        });

        m_button_btv.onClick.AddListener(() =>
        {
            Application.OpenURL(biliUrl);
        });
        m_button_github.onClick.AddListener(() =>
        {
            Application.OpenURL(githubUrl);
        });

        m_text_version.text = Application.version;

        m_button_redX.onClick.AddListener(() =>
        {
            //关闭自身
        });
    }
    #endregion
}