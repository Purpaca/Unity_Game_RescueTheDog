using UnityEngine;
using UnityEngine.UI;
using Purpaca;

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

    protected override void Awake()
    {
        base.Awake();

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
            Application.OpenURL("https://space.bilibili.com/3546697445673471");
        });
        m_button_github.onClick.AddListener(() =>
        {
            Application.OpenURL("https://github.com/Purpaca/Unity_Game_RescueTheDog");
        });

        m_text_version.text = Application.version;

        m_button_redX.onClick.AddListener(() =>
        {
            //关闭自身
        });
    }
}