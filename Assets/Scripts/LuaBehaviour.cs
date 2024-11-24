using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

/// <summary>
/// 由Lua脚本驱动的MonoBehaviour
/// </summary>
public class LuaBehaviour : MonoBehaviour
{
    [Space, SerializeField]
    private string luaModuleName;

    private void Awake()
    {
        LuaTable meta = XluaManager.DoString($"require('{luaModuleName}')")[0] as LuaTable;
        //XluaManager.LuaEnv.NewTable()
    }
}