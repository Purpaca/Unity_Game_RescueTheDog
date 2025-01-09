using System.IO;
using UnityEngine;

/// <summary>
/// 用户进度信息
/// </summary>
[System.Serializable]
public class UserProgressData
{
    private int m_passedLevelCount;

    #region 构造器
    public UserProgressData() 
    {
        m_passedLevelCount = 0;
    }
    #endregion

    #region 属性
    /// <summary>
    /// 存档文件的存放路径
    /// </summary>
    public static string StoragePath
    {
        get => Path.Combine(Application.persistentDataPath, "user_archive.json");
    }

    /// <summary>
    /// 通关数
    /// </summary>
    public int PassedLevelCount 
    {
        get => m_passedLevelCount;
        set => m_passedLevelCount = Mathf.Max(value, 0);
    }
    #endregion
}