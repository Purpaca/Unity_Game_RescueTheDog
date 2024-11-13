using Singleton;
using XLua;

public class XluaManager : AutomaticSingletonMonoBehaviour<XluaManager>
{
    private LuaEnv luaEnv;

    #region 属性
    public static LuaTable Global 
    {
        get => instance.luaEnv.Global;
    }
    #endregion

    #region Public 方法
    public static object[] DoString(string chunk, string chunkName = "chunk", LuaTable env = null) 
    {
        return instance.luaEnv.DoString(chunk, chunkName, env);
    }

    public static object[] DoString(byte[] chunk, string chunkName = "chunk", LuaTable env = null) 
    {
        return instance.luaEnv.DoString(chunk, chunkName, env);
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
            var asset = AssetManager.GetInstance().LoadLuaScript(path);
            return asset.bytes;
        });
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