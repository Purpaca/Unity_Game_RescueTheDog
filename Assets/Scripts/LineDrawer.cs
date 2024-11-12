using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 线条绘制器
/// </summary>
public class LineDrawer : MonoBehaviour
{
    [SerializeField, Tooltip("遮挡层")]
    private LayerMask m_blockedByLayer = 0;

    [Space, SerializeField, Min(0.01f), Tooltip("线条每个端点之间的距离(此值越小，线条越精细，但性能消耗也会相对增加)")]
    private float m_distancePerVert = 0.1f;
    [SerializeField, Min(2)]
    private int m_leastLineEndpointsNumber = 2;
    [SerializeField]
    private float m_lineZPosition;

    [Space, SerializeField, Min(0.01f)]
    private float m_thickness = 1.0f;

    [Space, SerializeField, ColorUsage(true)]
    private Color m_lineColor = Color.white;
    [SerializeField]
    private Material m_lineMaterial;

    private GameObject _line;
    private LineRenderer _lineRenderer;
    private PolygonCollider2D _linePolygonCollider;
    private System.Collections.Generic.List<Vector3> _points = new System.Collections.Generic.List<Vector3>();

    private bool isDrawing = false;
    private UnityAction<GameObject> onDrawEnd;

    #region 属性
    /// <summary>
    /// 遮挡线条绘画的层，无法在被遮挡的层上绘制线条
    /// </summary>
    public LayerMask BlockedByLayer
    {
        get => m_blockedByLayer;
        set => m_blockedByLayer = value;
    }

    /// <summary>
    /// 画出的线条的颜色
    /// </summary>
    public Color Color
    {
        get => m_lineColor;
        set => m_lineColor = value;
    }

    /// <summary>
    /// 画出的线条所使用的材质
    /// </summary>
    public Material Material
    {
        get => m_lineMaterial;
        set => m_lineMaterial = value;
    }

    /// <summary>
    /// 画出的线条的Z轴坐标
    /// </summary>
    public float ZAxisPosition
    {
        get => m_lineZPosition;
        set => m_lineZPosition = value;
    }

    /// <summary>
    /// 画出的线条的宽度
    /// </summary>
    public float Thickness
    {
        get => m_thickness;
        set => m_thickness = Mathf.Clamp(value, 0.01f, value);
    }

    /// <summary>
    /// 绘制出的线条每个端点的距离
    /// </summary>
    public float PointsDistance
    {
        get => m_distancePerVert;
        set => m_distancePerVert = value;
    }

    /// <summary>
    /// 绘制出的线条的最小端点数
    /// </summary>
    public int LeastEndPointsCount
    {
        get => m_leastLineEndpointsNumber;
        set
        {
            m_leastLineEndpointsNumber = Mathf.Max(2, value);
        }
    }
    #endregion

    #region Public 方法
    /// <summary>
    /// 设置完成绘制一条线条后的回调方法
    /// </summary>
    public void SetOnDrawEndCallback(UnityAction<GameObject> callback)
    {
        onDrawEnd = callback;
    }

    /// <summary>
    /// 添加完成绘制一条线条后的回调方法
    /// </summary>
    public void AddOnDrawEndCallback(UnityAction<GameObject> callback) 
    {
        onDrawEnd += callback;
    }

    /// <summary>
    /// 清除完成绘制一条线条后的回调方法
    /// </summary>
    public void ClearOnDrawEndCallback()
    {
        onDrawEnd = null;
    }
    #endregion

    #region Private 消息
    /// <summary>
    /// 更新点数据到LineRenderer
    /// </summary>
    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = _points.Count;
        _lineRenderer.SetPositions(_points.ToArray());
    }

    /// <summary>
    /// 更新Polygon顶点信息到PolyonCollider2D
    /// </summary>
    private void UpdatePolygonCollider2D()
    {
        System.Collections.Generic.List<Vector2> leftVerts = new System.Collections.Generic.List<Vector2>();
        System.Collections.Generic.List<Vector2> rightVerts = new System.Collections.Generic.List<Vector2>();

        //求出第一到倒数第二个端点对应的顶点
        for (int i = 0; i < _points.Count - 1; i++)
        {
            Vector2 point = new Vector2(_points[i].x, _points[i].y);
            Vector2 nextpoint = new Vector2(_points[i + 1].x, _points[i + 1].y);

            Vector2 normal = (nextpoint - point).normalized;

            Vector2 leftVert = -(Vector2.Perpendicular(normal) * m_thickness / 2) + point;
            Vector2 rightVert = (Vector2.Perpendicular(normal) * m_thickness / 2) + point;

            leftVerts.Add(leftVert);
            rightVerts.Add(rightVert);
        }

        //求出最后一个端点对应的顶点，因为法线是反向的，所以所求出的左右顶点是相反的
        Vector2 lastpoint = new Vector2(_points[_points.Count - 1].x, _points[_points.Count - 1].y);
        Vector2 prepoint = new Vector2(_points[_points.Count - 2].x, _points[_points.Count - 2].y);
        Vector2 nml = (prepoint - lastpoint).normalized;    //此处求出的法线是反向的
        Vector2 left = (Vector2.Perpendicular(nml) * m_thickness / 2) + lastpoint;
        Vector2 right = -(Vector2.Perpendicular(nml) * m_thickness / 2) + lastpoint;

        leftVerts.Add(left);
        rightVerts.Add(right);

        rightVerts.Reverse();
        System.Collections.Generic.List<Vector2> vertsInOrder = new System.Collections.Generic.List<Vector2>();
        vertsInOrder.AddRange(leftVerts);
        vertsInOrder.AddRange(rightVerts);

        _linePolygonCollider.points = vertsInOrder.ToArray();
    }
    #endregion

    #region Unity 消息
    private void Update()
    {
        if (isDrawing)
        {
            if (Input.GetMouseButtonUp(0))   //结束画线
            {
                isDrawing = false;

                if (_points.Count <= m_leastLineEndpointsNumber)   //如果线条的端点数不达到设定的最小值，则忽略本次的画线行为并销毁意外的线条
                {
                    Destroy(_line);
                    _line = null;
                    _lineRenderer = null;
                    _linePolygonCollider = null;
                }
                else
                {
                    UpdatePolygonCollider2D();
                    _lineRenderer.useWorldSpace = false;

                    onDrawEnd?.Invoke(_line);

                    _line = null;
                    _lineRenderer = null;
                    _linePolygonCollider = null;
                }

                return;
            }

            //判断当前用户于屏幕上的绘画点是否被 不可绘制层 遮挡
            Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (((1 << colliders[i].gameObject.layer) & m_blockedByLayer) > 0)
                {
                    return;
                }
            }

            //判断当前用户于屏幕上的绘画点与上一点的连线间是否有被设置为 不可绘制层 的遮挡物
            RaycastHit2D[] hits = Physics2D.LinecastAll(_points[_points.Count - 1], Camera.main.ScreenToWorldPoint(Input.mousePosition));
            for (int i = 0; i < hits.Length; i++)
            {
                if (((1 << hits[i].transform.gameObject.layer) & m_blockedByLayer) > 0)
                {
                    return;
                }
            }

            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = m_lineZPosition;
            if (Vector3.Distance(_points[_points.Count - 1], point) >= m_distancePerVert)
            {
                _points.Add(point);
                UpdateLineRenderer();
            }
            else if (_points.Count > 1)    //如果用户当前企图绘制的点与上一个点的夹角足够锐利，则将当前的点位置作为转角点
            {
                Vector2 dir = (point - _points[_points.Count - 1]).normalized;
                Vector2 lastDir = (_points[_points.Count - 1] - _points[_points.Count - 2]).normalized;

                if (Vector2.Angle(dir, lastDir) < 90)
                {
                    _points.Add(point);
                    UpdateLineRenderer();
                }
            }

        }
        else if (Input.GetMouseButton(0))   //刚开始画线
        {
            //判断当前用户于屏幕上的绘画点是否被 不可绘制层 遮挡
            Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (((1 << colliders[i].gameObject.layer) & m_blockedByLayer) > 0)
                {
                    return;
                }
            }

            _points.Clear();
            isDrawing = true;

            _line = new GameObject("line");

            _lineRenderer = _line.AddComponent<LineRenderer>();
            _lineRenderer.material = m_lineMaterial;
            _lineRenderer.startColor = m_lineColor;
            _lineRenderer.endColor = m_lineColor;
            _lineRenderer.widthMultiplier = m_thickness;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.numCapVertices = 6;

            _linePolygonCollider = _line.AddComponent<PolygonCollider2D>();
            _linePolygonCollider.isTrigger = false;

            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = m_lineZPosition;
            _points.Add(point);
            UpdateLineRenderer();
        }
    }

    private void OnDisable()
    {
        if (isDrawing)
        {
            Destroy(_line);
            isDrawing = false;
        }
    }
    #endregion
}