using UnityEngine;
using UnityEngine.Events;
using AssetManagement;
using Newtonsoft.Json;

/// <summary>
/// 资源管理器
/// </summary>
public class AssetManager : AssetBundleManager
{
    private const string AssetBundleName_BeePrefab = "persona_bee/bundle";
    private const string AssetBundleName_CombPrefab = "persona_comb/bundle";
    private const string AssetBundleName_DogPrefab = "persona_dog/bundle";
    private const string AssetBundleName_Font = "font";
    private const string AssetBundleName_Level = "level/bundle";
    private const string AssetBundleName_Lua = "script";
    private const string AssetBundleName_Music = "music";
    private const string AssetBundleName_Sound = "sound";

    #region Public 方法
    /// <summary>
    /// 获取资源管理器的单例实例
    /// </summary>
    /// <returns></returns>
    public static AssetManager GetInstance() 
    {
        return instance as AssetManager;
    }

    /// <summary>
    /// 加载指定名称的蜂预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂预制体资源的名称</param>
    public GameObject LoadBeePrefab(string assetName) 
    {
        return LoadAsset<GameObject>(AssetBundleName_BeePrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的蜂预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂预制体资源的名称</param>
    public AssetBundleRequest LoadBeePrefabAsync(string assetName)
    {
        return LoadAssetAsync<GameObject>(AssetBundleName_BeePrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的蜂预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂预制体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public void LoadBeePrefabAsync(string assetName, UnityAction<GameObject> callback)
    {
        LoadAssetAsync(AssetBundleName_BeePrefab, assetName, callback);
    }

    /// <summary>
    /// 加载指定名称的蜂巢预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂巢预制体资源的名称</param>
    public GameObject LoadCombPrefab(string assetName) 
    {
        return LoadAsset<GameObject>(AssetBundleName_CombPrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的蜂巢预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂巢预制体资源的名称</param>
    public AssetBundleRequest LoadCombPrefabAsync(string assetName)
    {
        return LoadAssetAsync<GameObject>(AssetBundleName_CombPrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的蜂巢预制体资源
    /// </summary>
    /// <param name="assetName">要加载的蜂巢预制体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public void LoadCombPrefabAsync(string assetName, UnityAction<GameObject> callback)
    {
        LoadAssetAsync(AssetBundleName_CombPrefab, assetName, callback);
    }

    /// <summary>
    /// 加载指定名称的狗头预制体资源
    /// </summary>
    /// <param name="assetName">要加载的狗头预制体资源的名称</param>
    public GameObject LoadDogPrefab(string assetName)
    {
        return LoadAsset<GameObject>(AssetBundleName_DogPrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的狗头预制体资源
    /// </summary>
    /// <param name="assetName">要加载的狗头预制体资源的名称</param>
    public AssetBundleRequest LoadDogPrefabAsync(string assetName)
    {
        return LoadAssetAsync<GameObject>(AssetBundleName_DogPrefab, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的狗头预制体资源
    /// </summary>
    /// <param name="assetName">要加载的狗头预制体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public void LoadDogPrefabAsync(string assetName, UnityAction<GameObject> callback)
    {
        LoadAssetAsync(AssetBundleName_DogPrefab, assetName, callback);
    }

    /// <summary>
    /// 加载字体资源
    /// </summary>
    /// <param name="assetName">要加载的字体资源的名称</param>
    public Font LoadFontAsset(string assetName) 
    {
        return LoadAsset<Font>(AssetBundleName_Font, assetName);
    }

    /// <summary>
    /// 异步加载字体资源
    /// </summary>
    /// <param name="assetName">要加载的字体资源的名称</param>
    public AssetBundleRequest LoadFontAssetAsync(string assetName)
    {
        return LoadAssetAsync<Font>(AssetBundleName_Font, assetName);
    }

    /// <summary>
    /// 异步加载字体资源
    /// </summary>
    /// <param name="assetName">要加载的字体资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public void LoadFontAssetAsync(string assetName, UnityAction<Font> callback)
    {
        LoadAssetAsync(AssetBundleName_Font, assetName, callback);
    }

    /// <summary>
    /// 加载指定名称的关卡配置ScriptingObject资源
    /// </summary>
    /// <param name="assetName">要加载的关卡配置ScriptingObject资源的名称</param>
    public LevelConfig LoadLevelConfig(string assetName)
    {
        return LoadAsset<LevelConfig>(AssetBundleName_Level, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的关卡配置ScriptingObject资源
    /// </summary>
    /// <param name="assetName">要加载的关卡配置ScriptingObject资源的名称</param>
    public AssetBundleRequest LoadLevelConfigAsync(string assetName)
    {
        return LoadAssetAsync<LevelConfig>(AssetBundleName_Level, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的关卡配置ScriptingObject资源
    /// </summary>
    /// <param name="assetName">要加载的关卡配置ScriptingObject资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public void LoadLevelConfigAsync(string assetName, UnityAction<LevelConfig> callback)
    {
        LoadAssetAsync(AssetBundleName_Level, assetName, callback);
    }

    /// <summary>
    /// 加载关卡配置ScriptingObject资源的名称列表
    /// </summary>
    public string[] LoadLevelList()
    {
        var asset = LoadAsset<TextAsset>(AssetBundleName_Level, "list");
        return JsonConvert.DeserializeObject<string[]>(asset.text);
    }

    /// <summary>
    /// 加载Lua脚本资源
    /// </summary>
    /// <param name="assetName">要加载的Lua脚本资源的名称</param>
    public TextAsset LoadLuaScript(string assetName)
    {
        return LoadAsset<TextAsset>(AssetBundleName_Lua, assetName);
    }

    /// <summary>
    /// 加载指定名称的音乐资源
    /// </summary>
    /// <param name="assetName">要加载的音乐资源的名称</param>
    public AudioClip LoadMusic(string assetName)
    {
        return LoadAsset<AudioClip>(AssetBundleName_Music, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的音乐资源
    /// </summary>
    /// <param name="assetName">要加载的音乐资源的名称</param>
    public AssetBundleRequest LoadMusicAsync(string assetName)
    {
        return LoadAssetAsync<AudioClip>(AssetBundleName_Music, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的音乐资源
    /// </summary>
    /// <param name="assetName">要加载的音乐资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public void LoadMusicAsync(string assetName, UnityAction<AudioClip> callback)
    {
        LoadAssetAsync(AssetBundleName_Music, assetName, callback);
    }

    /// <summary>
    /// 加载音乐资源的名称列表
    /// </summary>
    /// <returns></returns>
    public string[] LoadMusicList() 
    {
        string[] list = null;

        var asset = LoadAsset<TextAsset>(AssetBundleName_Music, "mus_list");
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
    public AudioClip LoadSound(string assetName)
    {
        return LoadAsset<AudioClip>(AssetBundleName_Sound, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的音效资源
    /// </summary>
    /// <param name="assetName">要加载的音效资源的名称</param>
    public AssetBundleRequest LoadSoundAsync(string assetName)
    {
        return LoadAssetAsync<AudioClip>(AssetBundleName_Sound, assetName);
    }

    /// <summary>
    /// 异步加载指定名称的音效资源
    /// </summary>
    /// <param name="assetName">要加载的音效资源的名称</param>
    /// <param name="callback">资源加载请求结束后的回调方法</param>
    public void LoadSoundAsync(string assetName, UnityAction<AudioClip> callback)
    {
        LoadAssetAsync(AssetBundleName_Sound, assetName, callback);
    }
    #endregion
}