using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private LineDrawer m_lineDrawer;

    private UserSettingsData m_userSettingData;
    private UserProgressData m_userProgressData;

    private GameObject _dogPrefabAsset;
    private GameObject _combPrefabAsset;

    private LevelConfig _levelConfig;
    private GameObject _currentTerrain;
    private List<DogController> _spawnedDogs;
    private List<CombController> _spawnedCombs;
    private GameObject _drawedLine;

    #region 属性
    /// <summary>
    /// GameManager的单例实例
    /// </summary>
    public static GameManager Instance { get => instance; }

    /// <summary>
    /// 当前持有的线绘制器
    /// </summary>
    public LineDrawer LineDrawer { get => m_lineDrawer; }

    /// <summary>
    /// 用户设置信息
    /// </summary>
    public UserSettingsData UserSettingsData
    {
        get
        {
            if (m_userSettingData == null)
            {
                m_userSettingData = new UserSettingsData();
            }
            return m_userSettingData;
        }
        set => m_userSettingData = value;
    }

    /// <summary>
    /// 用户进度信息
    /// </summary>
    public UserProgressData UserProgressData
    {
        get
        {
            if (m_userProgressData == null)
            {
                m_userProgressData = new UserProgressData();
            }
            return m_userProgressData;
        }
        set => m_userProgressData = value;
    }
    #endregion

    #region Public 方法
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame() 
    {
        ClearLevel();
        BuildLevel(_levelConfig);
        m_lineDrawer.enabled = true;
    }
    #endregion

    #region Private 方法
    /// <summary>
    /// 根据给定的关卡配置信息构建关卡
    /// </summary>
    private void BuildLevel(LevelConfig config)
    {
        _currentTerrain = Instantiate(config.Terrain);
        _currentTerrain.transform.position = new Vector3(0, 0, -3.0f);

        foreach (var dogInfo in config.Dogs)
        {
            DogController dog = Instantiate(_dogPrefabAsset).GetComponent<DogController>();
            dog.transform.position = new Vector3(dogInfo.Position.x, dogInfo.Position.y, -2.0f);
            dog.SetDogeSimulated(false);
            _spawnedDogs.Add(dog);
        }

        foreach (var combInfo in config.Combs)
        {
            CombController comb = Instantiate(_combPrefabAsset).GetComponent<CombController>();
            comb.SetBeesToEmit(combInfo.bees);

            comb.transform.position = new Vector3(combInfo.positon.x, combInfo.positon.y, -1.0f);
            Vector3 pos = comb.transform.localScale;
            pos.x = combInfo.direction == LevelConfig.CombInfo.CombFacingDirection.Left ? -1 : 1;
            comb.transform.localScale = pos;

            _spawnedCombs.Add(comb);
        }

        m_lineDrawer.enabled = true;
        m_lineDrawer.OnDrawEnd.AddListener((line) =>
        {
            Rigidbody2D lineRigidBody = line.AddComponent<Rigidbody2D>();
            lineRigidBody.mass = 0.1f;
            lineRigidBody.simulated = true;

            _drawedLine = line;

            foreach (var doge in _spawnedDogs)
            {
                doge.SetDogeSimulated(true);
            }
            foreach (var comb in _spawnedCombs)
            {
                comb.Emit();
            }

            m_lineDrawer.enabled = false;
        });
    }

    private void ClearLevel()
    {
        m_lineDrawer.enabled = false;
        m_lineDrawer.OnDrawEnd.RemoveAllListeners();

        for (int i = _spawnedDogs.Count - 1; i >= 0; i--)
        {
            var doge = _spawnedDogs[i];
            _spawnedDogs.RemoveAt(i);
            Destroy(doge.gameObject);
        }

        for (int i = _spawnedCombs.Count - 1; i >= 0; i--)
        {
            var comb = _spawnedCombs[i];
            _spawnedCombs.RemoveAt(i);
            Destroy(comb.gameObject);
        }

        if (_currentTerrain != null)
        {
            Destroy(_currentTerrain);
        }
        if (_drawedLine != null)
        {
            Destroy(_drawedLine);
        }
    }
    #endregion

    #region Unity 消息
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        m_lineDrawer.enabled = false;
        _spawnedDogs = new List<DogController>();
        _spawnedCombs = new List<CombController>();
    }
    #endregion
}