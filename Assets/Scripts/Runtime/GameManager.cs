using System.Collections.Generic;
using UnityEngine;
using Purpaca;
using Singleton;
using IEnumerator = System.Collections.IEnumerator;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private LineDrawer m_lineDrawer;

    private UserSettingsData m_userSettingData;
    private UserProgressData m_userProgressData;

    private string[] m_musicList;
    private int _musicID;
    private string _musicHandle;

    private GameObject _dogPrefabAsset;
    private GameObject _combPrefabAsset;

    private bool isGameFailed = false;
    private Coroutine _gameProcess;

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
    /// 音乐的播放音量
    /// </summary>
    public static float MusicVolume 
    {
        get => AudioManager.MusicVolume;
        set => AudioManager.MusicVolume = value;
    }

    /// <summary>
    /// 音效的播放音量
    /// </summary>
    public static float SoundVolume 
    {
        get => AudioManager.SoundVolume;
        set => AudioManager.SoundVolume = value;
    }

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
    /// 随机播放BGM
    /// </summary>
    public void PlayMusic()
    {
        if (!string.IsNullOrEmpty(_musicHandle))
        {
            AudioManager.Free(_musicHandle);
        }

        _musicID = (_musicID + Random.Range(1, 2)) % m_musicList.Length;
        AudioClip clip = AssetManager.LoadMusic(m_musicList[_musicID]);
        _musicHandle = AudioManager.Play(clip, -1, 1.0f, AudioManager.OutputChannel.Music);
    }

    /// <summary>
    /// 停止随机播放的BGM
    /// </summary>
    public void StopMusic()
    {
        if (!string.IsNullOrEmpty(_musicHandle))
        {
            AudioManager.Free(_musicHandle);
        }
        _musicHandle = string.Empty;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame(LevelConfig levelConfig) 
    {
        isGameFailed = false;
        ClearLevel();
        BuildLevel(levelConfig);
        m_lineDrawer.enabled = true;

        if(_gameProcess != null) 
        {
            StopCoroutine(_gameProcess);
        }
        _gameProcess = StartCoroutine(GameProcess());
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

            dog.AddOnDogeInjuredListener(() =>
            {

            });

            _spawnedDogs.Add(dog);
        }

        foreach (var combInfo in config.Combs)
        {
            CombController comb = Instantiate(_combPrefabAsset).GetComponent<CombController>();
            comb.SetBeesToEmit(combInfo.bees);

            comb.transform.position = new Vector3(combInfo.position.x, combInfo.position.y, -1.0f);
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

    #region 协程
    private IEnumerator GameProcess() 
    {
        float timer = 10.0f;
        while (!isGameFailed) 
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("赢了家人们");
                StopCoroutine(_gameProcess);
            }

            yield return null;
        }

        Debug.Log("输了家人们");
    }
    #endregion

    #region Unity 消息
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        m_musicList = AssetManager.LoadMusicList();
        _musicID = Random.Range(0, m_musicList.Length - 1);

        // 这里先用Magic string吧...等以后有机会实现换装功能再优化
        _dogPrefabAsset = AssetManager.LoadDogPrefab("dog_vanilla");
        _combPrefabAsset = AssetManager.LoadCombPrefab("comb_vanilla");

        m_lineDrawer.enabled = false;
        _spawnedDogs = new List<DogController>();
        _spawnedCombs = new List<CombController>();
    }
    #endregion
}