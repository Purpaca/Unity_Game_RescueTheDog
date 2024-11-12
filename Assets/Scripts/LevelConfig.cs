using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName ="Create Level Config", fileName ="New GameLevel")]
public class LevelConfig : ScriptableObject
{
    /// <summary>
    /// 关卡的地形预制体
    /// </summary>
    [SerializeField, Tooltip("关卡的地形预制体")]
    public GameObject terrain;

    /// <summary>
    /// 关卡所有的狗狗信息
    /// </summary>
    [SerializeField, Tooltip("关卡所有的狗狗信息")]
    public List<DogInfo> dogs;

    /// <summary>
    /// 关卡所有的蜂巢信息
    /// </summary>
    [SerializeField, Tooltip("关卡所有的蜂巢信息")]
    public List<CombInfo> combs;

    /// <summary>
    /// 关卡帮助示意图
    /// </summary>
    [SerializeField, Tooltip("关卡帮助示图")]
    public Texture2D diagram;

    #region 内部类型
    [System.Serializable]
    public class DogInfo
    {
        public Vector2 position;
    }

    /// <summary>
    /// 蜂巢信息
    /// </summary>
    [System.Serializable]
    public class CombInfo
    {
        /// <summary>
        /// 此蜂巢的位置
        /// </summary>
        [SerializeField, Tooltip("蜂巢的位置")]
        public Vector2 positon;

        /// <summary>
        /// 此蜂巢的朝向
        /// </summary>
        [SerializeField, Tooltip("蜂巢的朝向")]
        public CombFacingDirection direction;

        /// <summary>
        /// 此蜂巢要发射的蜜蜂信息
        /// </summary>
        [Space, SerializeField, Tooltip("蜂巢要发射的蜜蜂")]
        public List<CombController.BeeInfo> bees;

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