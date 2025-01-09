using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using AssetManagement;

/// <summary>
/// 资源管理器
/// </summary>
public class AssetManager
{
    #region AssetBundle 路径
    private const string AssetBundleName_BeePrefab = "persona_bee/bundle";
    private const string AssetBundleName_CombPrefab = "persona_comb/bundle";
    private const string AssetBundleName_DogPrefab = "persona_dog/bundle";
    private const string AssetBundleName_Font = "font";
    private const string AssetBundleName_Level = "level/bundle";
    private const string AssetBundleName_Lua = "script";
    private const string AssetBundleName_Music = "music";
    private const string AssetBundleName_Sound = "sound";
    private const string AssetBundleName_UI = "ui/bundle";
    #endregion

    #region Public 方法
    /// <summary>
    /// 加载指定名称的蜂预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂预制体资源的名称</param>
    public static GameObject LoadBeePrefab(string assetName) 
    {
        return AssetBundleManager.Instance.LoadAsset<GameObject>(AssetBundleName_BeePrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的蜂预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂预制体资源的名称</param>
    public static AssetBundleRequest LoadBeePrefabAsync(string assetName)
    {
        return AssetBundleManager.Instance.LoadAssetAsync<GameObject>(AssetBundleName_BeePrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的蜂预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂预制体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public static void LoadBeePrefabAsync(string assetName, UnityAction<GameObject> callback)
    {
        AssetBundleManager.Instance.LoadAssetAsync(AssetBundleName_BeePrefab, assetName, callback);
    }

    /// <summary>
    /// 加载指定名称的蜂巢预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂巢预制体资源的名称</param>
    public static GameObject LoadCombPrefab(string assetName) 
    {
        return AssetBundleManager.Instance.LoadAsset<GameObject>(AssetBundleName_CombPrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的蜂巢预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂巢预制体资源的名称</param>
    public static AssetBundleRequest LoadCombPrefabAsync(string assetName)
    {
        return AssetBundleManager.Instance.LoadAssetAsync<GameObject>(AssetBundleName_CombPrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的蜂巢预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂巢预制体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public static void LoadCombPrefabAsync(string assetName, UnityAction<GameObject> callback)
    {
        AssetBundleManager.Instance.LoadAssetAsync(AssetBundleName_CombPrefab, assetName, callback);
    }

    /// <summary>
    /// 加载指定名称的狗头预制体资源
    /// </summary>
    /// <param name="assetName">要加载的狗头预制体资源的名称</param>
    public static GameObject LoadDogPrefab(string assetName)
    {
        return AssetBundleManager.Instance.LoadAsset<GameObject>(AssetBundleName_DogPrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的狗头预制体资源
    /// </summary>
    /// <param name="assetName">要加载的狗头预制体资源的名称</param>
    public static AssetBundleRequest LoadDogPrefabAsync(string assetName)
    {
        return AssetBundleManager.Instance.LoadAssetAsync<GameObject>(AssetBundleName_DogPrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的狗头预制体资源
    /// </summary>
    /// <param name="assetName">要加载的狗头预制体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public static void LoadDogPrefabAsync(string assetName, UnityAction<GameObject> callback)
    {
        AssetBundleManager.Instance.LoadAssetAsync(AssetBundleName_DogPrefab, assetName, callback);
    }

    /// <summary>
    /// 加载字体资源
    /// </summary>
    /// <param name="assetName">要加载的字体资源的名称</param>
    public static Font LoadFontAsset(string assetName) 
    {
        return AssetBundleManager.Instance.LoadAsset<Font>(AssetBundleName_Font, assetName);
    }

    /// <summary>
    /// 异步加载字体资源
    /// </summary>
    /// <param name="assetName">要加载的字体资源的名称</param>
    public static AssetBundleRequest LoadFontAssetAsync(string assetName)
    {
        return AssetBundleManager.Instance.LoadAssetAsync<Font>(AssetBundleName_Font, assetName);
    }

    /// <summary>
    /// 异步加载字体资源
    /// </summary>
    /// <param name="assetName">要加载的字体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public static void LoadFontAssetAsync(string assetName, UnityAction<Font> callback)
    {
        AssetBundleManager.Instance.LoadAssetAsync(AssetBundleName_Font, assetName, callback);
    }

    /// <summary>
    /// 加载指定名称的关卡配置ScriptingObject资源
    /// </summary>
    /// <param name="assetName">要加载的关卡配置ScriptingObject资源的名称</param>
    public static LevelConfig LoadLevelConfig(string assetName)
    {
        return AssetBundleManager.Instance.LoadAsset<LevelConfig>(AssetBundleName_Level, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的关卡配置ScriptingObject资源
    /// </summary>
    /// <param name="assetName">要加载的关卡配置ScriptingObject资源的名称</param>
    public static AssetBundleRequest LoadLevelConfigAsync(string assetName)
    {
        return AssetBundleManager.Instance.LoadAssetAsync<LevelConfig>(AssetBundleName_Level, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的关卡配置ScriptingObject资源
    /// </summary>
    /// <param name="assetName">要加载的关卡配置ScriptingObject资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public static void LoadLevelConfigAsync(string assetName, UnityAction<LevelConfig> callback)
    {
        AssetBundleManager.Instance.LoadAssetAsync(AssetBundleName_Level, assetName, callback);
    }

    /// <summary>
    /// 加载关卡配置ScriptingObject资源的名称列表
    /// </summary>
    public static string[] LoadLevelList()
    {
        var asset = AssetBundleManager.Instance.LoadAsset<TextAsset>(AssetBundleName_Level, "list");
        return JsonConvert.DeserializeObject<string[]>(asset.text);
    }

    /// <summary>
    /// 加载Lua脚本资源
    /// </summary>
    /// <param name="assetName">要加载的Lua脚本资源的名称</param>
    public static TextAsset LoadLuaScript(string assetName)
    {
        return AssetBundleManager.Instance.LoadAsset<TextAsset>(AssetBundleName_Lua, assetName);
    }

    /// <summary>
    /// 加载指定名称的音乐资源
    /// </summary>
    /// <param name="assetName">要加载的音乐资源的名称</param>
    public static AudioClip LoadMusic(string assetName)
    {
        return AssetBundleManager.Instance.LoadAsset<AudioClip>(AssetBundleName_Music, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的音乐资源
    /// </summary>
    /// <param name="assetName">要加载的音乐资源的名称</param>
    public static AssetBundleRequest LoadMusicAsync(string assetName)
    {
        return AssetBundleManager.Instance.LoadAssetAsync<AudioClip>(AssetBundleName_Music, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的音乐资源
    /// </summary>
    /// <param name="assetName">要加载的音乐资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public static void LoadMusicAsync(string assetName, UnityAction<AudioClip> callback)
    {
        AssetBundleManager.Instance.LoadAssetAsync(AssetBundleName_Music, assetName, callback);
    }

    /// <summary>
    /// 加载音乐资源的名称列表
    /// </summary>
    /// <returns></returns>
    public static string[] LoadMusicList() 
    {
        string[] list = null;

        var asset = AssetBundleManager.Instance.LoadAsset<TextAsset>(AssetBundleName_Music, "mus_list");
        if(asset != null && !string.IsNullOrEmpty(asset.text))
        {
            list = JsonConvert.DeserializeObject<string[]>(asset.text);
        }

        return list;
    }

    /// <summary>
    /// 加载指定名称的音效资源
    /// </summary>
    /// <param name="assetName">要加载的音效资源的名称</param>
    public static AudioClip LoadSound(string assetName)
    {
        return AssetBundleManager.Instance.LoadAsset<AudioClip>(AssetBundleName_Sound, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的音效资源
    /// </summary>
    /// <param name="assetName">要加载的音效资源的名称</param>
    public static AssetBundleRequest LoadSoundAsync(string assetName)
    {
        return AssetBundleManager.Instance.LoadAssetAsync<AudioClip>(AssetBundleName_Sound, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的音效资源
    /// </summary>
    /// <param name="assetName">要加载的音效资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public static void LoadSoundAsync(string assetName, UnityAction<AudioClip> callback)
    {
        AssetBundleManager.Instance.LoadAssetAsync(AssetBundleName_Sound, assetName, callback);
    }

    /// <summary>
    /// 加载指定名称的UI预制体资源
    /// </summary>
    /// <param name="assetName">要加载的UI预制体资源的名称</param>
    public static GameObject LoadUIPrefab(string assetName)
    {
        return AssetBundleManager.Instance.LoadAsset<GameObject>(AssetBundleName_UI, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的UI预制体资源
    /// </summary>
    /// <param name="assetName">要加载的UI预制体资源的名称</param>
    public static AssetBundleRequest LoadUIPrefabAsync(string assetName)
    {
        return AssetBundleManager.Instance.LoadAssetAsync<GameObject>(AssetBundleName_UI, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的UI预制体资源
    /// </summary>
    /// <param name="assetName">要加载的UI预制体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public static void LoadUIPrefabAsync(string assetName, UnityAction<GameObject> callback)
    {
        AssetBundleManager.Instance.LoadAssetAsync(AssetBundleName_UI, assetName, callback);
    }
    #endregion
}