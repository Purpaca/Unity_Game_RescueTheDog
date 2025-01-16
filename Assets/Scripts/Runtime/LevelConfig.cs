using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 关卡配置
/// </summary>
public class LevelConfig : ScriptableObject
{
    /// <summary>
    /// 关卡的地形预制体
    /// </summary>
    [SerializeField, Tooltip("关卡的地形预制体")]
    private GameObject terrain;

    /// <summary>
    /// 关卡所有的狗狗信息
    /// </summary>
    [SerializeField, Tooltip("关卡所有的狗狗信息")]
    private List<DogInfo> dogs;

    /// <summary>
    /// 关卡所有的蜂巢信息
    /// </summary>
    [SerializeField, Tooltip("关卡所有的蜂巢信息")]
    private List<CombInfo> combs;

    /// <summary>
    /// 关卡帮助示意图
    /// </summary>
    [SerializeField, Tooltip("关卡帮助示图")]
    private Texture2D diagram;

    #region 属性
    /// <summary>
    /// 关卡的地形预制体
    /// </summary>
    public GameObject Terrain { get => terrain; }

    /// <summary>
    /// 关卡所有的狗头信息
    /// </summary>
    public List<DogInfo> Dogs { get => dogs; }

    /// <summary>
    /// 关卡所有的蜂巢信息
    /// </summary>
    public List<CombInfo> Combs { get => combs; }

    /// <summary>
    /// 关卡帮助示意图
    /// </summary>
    public Texture2D Diagram {  get => diagram; }
    #endregion

    #region 内部类型
    /// <summary>
    /// 狗头信息
    /// </summary>
    [System.Serializable]
    public class DogInfo
    {
        /// <summary>
        /// 狗头的位置
        /// </summary>
        [SerializeField, Tooltip("狗头的位置")]
        private Vector2 position;

        public Vector2 Position { get => position; }
    }

    /// <summary>
    /// 蜂巢信息
    /// </summary>
    [System.Serializable]
    public class CombInfo
    {
        [SerializeField, Tooltip("蜂巢的位置")]
        public Vector2 positon;

        [SerializeField, Tooltip("蜂巢的朝向")]
        public CombFacingDirection direction;

        [Space, SerializeField, Tooltip("蜂巢要发射的蜜蜂")]
        public List<CombController.BeeInfo> bees;

        #region 属性
        /// <summary>
        /// 此蜂巢的位置
        /// </summary>
        public Vector2 Position { get => positon; }

        /// <summary>
        /// 蜂巢的朝向
        /// </summary>
        public CombFacingDirection Direction { get => direction; }

        /// <summary>
        /// 蜂巢要发射的蜜蜂信息
        /// </summary>
        public List<CombController.BeeInfo> Bees { get => bees; }
        #endregion

        /// <summary>
        /// 蜂巢的朝向
        /// </summary>
        [System.Serializable]
        public enum CombFacingDirection
        {
            Left,
            Right
        }
    }
    #endregion
}