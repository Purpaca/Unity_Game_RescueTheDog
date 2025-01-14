using Purpaca;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : UIPanel
{
    [SerializeField]
    private Slider m_slider_mus, m_slider_sfx;
    [Space, SerializeField]
    private Button m_button_safeArea;
    [Space, SerializeField] 
    private Button m_button_btv;
    [SerializeField]
    private Button m_button_github;
    [Space, SerializeField]
    private Button m_button_version;
    [Space, SerializeField]
    private Button m_button_redX;

    protected override void Awake()
    {
        base.Awake();

        m_slider_mus.onValueChanged.AddListener((value) => 
        {
            AudioManager.MusicVolume = value;
            GameManager.Instance.UserSettingsData.MusicVolume = value;
        });

        m_slider_sfx.onValueChanged.AddListener((value) =>
        {
            AudioManager.SoundVolume = value;
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

        var versionText = m_button_version.GetComponent<Text>();
        if(versionText != null)
        {
            versionText.text = Application.version;
        }

        m_button_version.onClick.AddListener(() => 
        {

        });

        m_button_redX.onClick.AddListener(() =>
        {
            //关闭自身
        });
    }
}