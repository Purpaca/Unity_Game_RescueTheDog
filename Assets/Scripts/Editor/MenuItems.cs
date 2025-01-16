using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// MenuItems存放
/// </summary>
public static class MenuItems
{
    /// <summary>
    /// 创建关卡配置信息
    /// </summary>
    [MenuItem("Rescue the Dog/Create LevelConfig")]
    public static void CreateLevelConfig()
    {
        LevelConfig asset = ScriptableObject.CreateInstance<LevelConfig>();

        string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(selectedPath))
        {
            selectedPath = "Assets";
        }

        string path = File.Exists(selectedPath) ? Path.GetDirectoryName(selectedPath) : selectedPath;
        path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, "level_unnamed.asset"));

        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}