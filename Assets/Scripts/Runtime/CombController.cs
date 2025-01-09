using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蜂巢行为控制器
/// </summary>
public class CombController : MonoBehaviour
{
    [SerializeField, Tooltip("局部坐标系下的蜜蜂发射点位")]
    private Vector2 m_emitPointInLocal;

    private List<BeeInfo> beesToSpawn;
    private List<BeeController> _spawnedBees;
    private Coroutine _coroutine;

    #region Public 方法
    /// <summary>
    /// 发射蜜蜂
    /// </summary>
    public void Emit()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        ClearBees();

        _coroutine = StartCoroutine(SpawnBees());
    }

    /// <summary>
    /// 清除场上所有的由当前蜂巢持有的蜜蜂
    /// </summary>
    public void ClearBees()
    {
        for (int i = _spawnedBees.Count - 1; i >= 0; i--)
        {
            var bee = _spawnedBees[i];
            _spawnedBees.Remove(bee);
            Destroy(bee.gameObject);
        }
    }

    /// <summary>
    /// 设置待发射蜜蜂的信息
    /// </summary>
    public void SetBeesToEmit(List<BeeInfo> bees)
    {
        beesToSpawn = bees;
    }
    #endregion

    #region Private 方法
    private IEnumerator SpawnBees()
    {
        var wait = new WaitForSeconds(0.1f);

        foreach (var b in beesToSpawn)
        {
            for (int i = 0; i < b.count; i++)
            {
                BeeController bee = Instantiate(b.prefab).GetComponent<BeeController>();

                Vector2 pos = transform.TransformPoint(m_emitPointInLocal);
                bee.transform.position = new Vector3(pos.x, pos.y, -2.0f);

                float theta = Random.value * 2 * Mathf.PI;
                // 生成一个随机的仰角（在-π/2到π/2之间）  
                float phi = Mathf.Acos(Random.value * 2 - 1) - Mathf.PI / 2;

                // 使用球面坐标转换为笛卡尔坐标  
                float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = Mathf.Sin(phi) * Mathf.Sin(theta);

                bee.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y).normalized * Random.Range(0, 10));
                _spawnedBees.Add(bee);

                yield return wait;
            }
        }
    }
    #endregion

    #region Unity 消息
    private void Awake()
    {
        _spawnedBees = new List<BeeController>();
    }

    private void OnDestroy()
    {
        ClearBees();
    }
    #endregion

    #region 内部类型
    [System.Serializable]
    public class BeeInfo
    {
        /// <summary>
        /// 蜜蜂的预制体
        /// </summary>
        [Tooltip("此种类蜜蜂的预制体")]
        public GameObject prefab;

        /// <summary>
        /// 蜜蜂的数量
        /// </summary>
        [Tooltip("此种类蜜蜂的数量")]
        public int count;
    }
    #endregion
}