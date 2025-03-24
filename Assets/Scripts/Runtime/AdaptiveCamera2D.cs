using UnityEngine;

/// <summary>
/// 会根据目标分辨率进行自适应以保证设置的画面内容有效区域能完整的显示的2D相机
/// </summary>
[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class AdaptiveCamera2D : MonoBehaviour
{
    [SerializeField, Tooltip("保证显示完整性的画面有效区方向")]
    private AdaptedAxis m_adaptedDisplayAxis = AdaptedAxis.Both;
    [SerializeField, Tooltip("Unity世界单位下的有效的画面区域大小")]
    private Vector2 m_contentSize = Vector2.one;

    /// <summary>
    /// 是否启用每帧都刷新相机设置以进行适配？
    /// </summary>
    [Space, SerializeField, Tooltip("是否启用每帧都刷新相机设置以进行适配（如果不启用，则只有当主动调用对应的方法时才会刷新相机设置以进行适配）")]
    public bool refreshAtEveryFrame = true;

    private Camera m_camera;

    #region 属性
    /// <summary>
    /// Unity世界单位下的有效的画面区域大小
    /// </summary>
    public Vector2 ContentSize
    {
        get => m_contentSize;
        set => m_contentSize = value;
    }

    /// <summary>
    /// 保证显示完整性的画面有效区方向
    /// </summary>
    public AdaptedAxis AdaptedDisplayAxis
    {
        get => m_adaptedDisplayAxis;
        set => m_adaptedDisplayAxis = value;
    }

    /// <summary>
    /// 适配的相机正交尺寸
    /// </summary>
    public float AdaptedCameraSize { get => GetAdaptedCameraSize(AdaptedDisplayAxis); }

    /// <summary>
    /// 画面有效区域的比例
    /// </summary>
    private float SafeAreaRatio { get => ContentSize.x / ContentSize.y; }

    private bool isEditMode { get => Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.OSXEditor; }
    #endregion

    #region Public 方法
    /// <summary>
    /// 使相机根据设置进行适配
    /// </summary>
    public void Adapt()
    {
        m_camera.orthographicSize = AdaptedCameraSize;
    }
    #endregion

    #region Private 方法
    /// <summary>
    /// 获取适配的相机正交尺寸
    /// </summary>
    /// <param name="axis">适配轴向</param>
    private float GetAdaptedCameraSize(AdaptedAxis axis)
    {
        switch (axis)
        {
            case AdaptedAxis.Vertical:
                return ContentSize.y / 2;
            case AdaptedAxis.Horizontal:
                return ContentSize.x / m_camera.aspect / 2;
            default:
                //如果为true，则横向扩展画面；如果为false，则竖向扩展画面
                return SafeAreaRatio <= m_camera.aspect ? GetAdaptedCameraSize(AdaptedAxis.Vertical) : GetAdaptedCameraSize(AdaptedAxis.Horizontal);
        }
    }
    #endregion

    #region Unity 消息
    private void Awake()
    {
        m_camera = GetComponent<Camera>();

        m_camera.orthographic = true;
    }

    private void Update()
    {
        if (isEditMode || refreshAtEveryFrame)
        {
            Adapt();
        }
    }
    #endregion

    #region 内部类型
    /// <summary>
    /// 保证显示完整性的画面有效区方向
    /// </summary>
    public enum AdaptedAxis
    {
        /// <summary>
        /// 竖直和水平方向都保证显示完整性
        /// </summary>
        Both,

        /// <summary>
        /// 保证水平方向的显示完整性
        /// </summary>
        Horizontal,

        /// <summary>
        /// 保证竖直方向的显示完整性
        /// </summary>
        Vertical
    }
    #endregion
}