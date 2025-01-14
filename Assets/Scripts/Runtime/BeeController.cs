using System.Collections;
using UnityEngine;

/// <summary>
/// 蜜蜂行为控制器
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BeeController : DogKillingCapable
{
    [SerializeField]
    private Sprite[] m_animatedSprites;

    [Space, SerializeField]
    public float m_maxMoveSpeed = 3;
    [SerializeField]
    public float m_moveForce = 2;
    [SerializeField]
    public float m_impactForce = 500;
    [SerializeField]
    public float m_detectRadius = 10.0f;

    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_spriteRenderer;

    private DogController _target;

    protected override bool IsBeeKilling => true;

    #region Public 方法
    /// <summary>
    /// 设置蜜蜂的目标
    /// </summary>
    /// <param name="target">目标</param>
    public void SetTarget(DogController target)
    {
        _target = target;
    }
    #endregion

    #region Private 方法
    private IEnumerator BeeMovementBehaviour()
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        Vector3 direction;

        while (true)
        {
            if (_target == null)
            {
                // 如果蜜蜂在执行行为期间没有被设置目标，则自行在范围内寻找目标
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_detectRadius);
                for (int i = 0; i < colliders.Length; i++)
                {
                    DogController dog = colliders[i].GetComponent<DogController>();
                    if (dog != null)
                    {
                        _target = dog;
                        break;
                    }
                }
            }
            else
            {
                // 若蜜蜂没有碰到障碍物（包括玩家绘制的线条以及狗狗）时，蜜蜂直接朝向着狗狗所在的位置移动
                direction = (_target.transform.position - transform.position).normalized;
                direction.z = 0;    // 清除目标于蜜蜂Z轴位置的影响，避免蜜蜂的图像出现诡异的立体感
                transform.right = direction;

                m_rigidbody.AddForce(m_moveForce * direction);
                m_rigidbody.velocity = Vector2.ClampMagnitude(m_rigidbody.velocity, m_maxMoveSpeed);
            }

            yield return waitForFixedUpdate;
        }
    }

    private IEnumerator BeeAnimation()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        WaitForSeconds waitSec = new WaitForSeconds(0.05f);

        int i = 0;
        while (true)
        {
            spriteRenderer.sprite = m_animatedSprites[i];
            i++;

            if (i >= m_animatedSprites.Length)
            {
                i = 0;
            }

            yield return waitSec;
        }
    }
    #endregion

    #region Unity 消息
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D (collision);

        // 若蜜蜂在前侧与左右侧碰撞到障碍物（包括玩家绘制的线条以及狗狗）时，蜜蜂会沿着当前碰撞点法线的切线方向随机移动
        if (collision.transform.GetComponent<BeeController>() == null)
        {
            if (Vector2.Angle(transform.right, collision.GetContact(0).point - (Vector2)transform.position) < 125)
            {
                Vector2 tangent = Vector2.Perpendicular(collision.GetContact(0).normal);
                m_rigidbody.AddForce((Random.value > 0.5f ? 1 : -1) * tangent * m_impactForce);
            }
        }
    }

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(BeeMovementBehaviour());
        StartCoroutine(BeeAnimation());
    }

    private void Update()
    {
        // 在蜜蜂的飞行水平朝向变化时翻转蜜蜂的图像，避免出现蜜蜂肚子向上躺飞的情况
        m_spriteRenderer.flipY = transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270;
    }
    #endregion
}