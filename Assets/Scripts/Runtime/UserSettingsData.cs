using System.IO;
using UnityEngine;

/// <summary>
/// 用户设置信息
/// </summary>
public class UserSettingsData
{
    private float m_musicVolume;
    private float m_sfxVolume;
    private float m_safeAreaOffsetUp;
    private float m_safeAreaOffsetDown;
    private float m_safeAreaOffsetLeft;
    private float m_safeAreaOffsetRight;

    #region 构造器
    public UserSettingsData()
    {
        m_musicVolume = 1.0f;
        m_sfxVolume = 1.0f;
        Utils.SafeAreaRectToOffsetValues(Screen.safeArea, out m_safeAreaOffsetUp, out m_safeAreaOffsetDown, out m_safeAreaOffsetLeft, out m_safeAreaOffsetRight);
    }
    #endregion

    #region 属性
    /// <summary>
    /// 配置文件的存放路径
    /// </summary>
    public static string StoragePath
    {
        get => Path.Combine(Application.persistentDataPath, "user_settings.json");
    }

    /// <summary>
    /// 音乐的音量
    /// </summary>
    public float MusicVolume
    {
        get { return m_musicVolume; }
        set { m_musicVolume = Mathf.Clamp(value, 0.0f, 1.0f); }
    }

    /// <summary>
    /// 音效的音量
    /// </summary>
    public float SfxVolume
    {
        get { return m_sfxVolume; }
        set { m_sfxVolume = Mathf.Clamp(value, 0.0f, 1.0f); }
    }

    /// <summary>
    /// 上 方向的安全区偏移值
    /// </summary>
    public float SafeAreaOffsetUp
    {
        get { return m_safeAreaOffsetUp; }
        set { m_safeAreaOffsetUp = Mathf.Clamp(value, 0.0f, 1.0f); }
    }

    /// <summary>
    /// 下 方向的安全区偏移值
    /// </summary>
    public float SafeAreaOffsetDown
    {
        get { return m_safeAreaOffsetDown; }
        set { m_safeAreaOffsetDown = Mathf.Clamp(value, 0.0f, 1.0f); }
    }

    /// <summary>
    /// 左 方向的安全区偏移值
    /// </summary>
    public float SafeAreaOffsetLeft
    {
        get { return m_safeAreaOffsetLeft; }
        set { m_safeAreaOffsetLeft = Mathf.Clamp(value, 0.0f, 1.0f); }
    }

    /// <summary>
    /// 右 方向的安全区偏移值
    /// </summary>
    public float SafeAreaOffsetRight
    {
        get { return m_safeAreaOffsetRight; }
        set { m_safeAreaOffsetRight = Mathf.Clamp(value, 0.0f, 1.0f); }
    }
    #endregion
}