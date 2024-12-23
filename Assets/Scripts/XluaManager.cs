using Singleton;
using UnityEngine;
using XLua;
using IEnumerator = System.Collections.IEnumerator;

public class XluaManager : AutoInstantiateMonoSingleton<XluaManager>
{
    private LuaEnv luaEnv;
    private float gcDuration = 60.0f;

    #region 属性
    public static LuaTable Global 
    {
        get => instance.luaEnv.Global;
    }
    #endregion

    #region Public 方法
    public static void Init() 
    {
        _ = instance;
    }

    public static object[] DoString(string chunk, string chunkName = "chunk", LuaTable env = null) 
    {
        return instance.luaEnv.DoString(chunk, chunkName, env);
    }

    public static object[] DoString(byte[] chunk, string chunkName = "chunk", LuaTable env = null) 
    {
        return instance.luaEnv.DoString(chunk, chunkName, env);
    }

    public static LuaTable NewTable()
    {
        return instance.luaEnv.NewTable();
    }
    #endregion

    #region Private 方法
    private IEnumerator GCWatcher()
    {
        while (true)
        {
            luaEnv.GC();
            yield return new WaitForSecondsRealtime(gcDuration);
        }
    }
    #endregion

    #region Unity 消息
    protected override void Awake()
    {
        base.Awake();
        luaEnv = new LuaEnv();

        // 添加基础自定义Loader - 从 AssetBundle 中加载
        luaEnv.AddLoader((ref string path) =>
        {
            var asset = AssetManager.LoadLuaScript(path);
            return asset.bytes;
        });

        StartCoroutine(GCWatcher());
    }

    private void Update()
    {
        luaEnv.Tick();
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
    #endregion
}