using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private LineDrawer lineDrawer;

    private GameObject _currentTerrain;
    private List<DogController> _spawnedDogs;
    private List<CombController> _spawnedCombs;
    private GameObject _drawedLine;

    #region Public 方法
    /// <summary>
    /// 根据给定的关卡配置信息构建关卡
    /// </summary>
    public void BuildLevel(LevelConfig config)
    {
        _currentTerrain = Instantiate(config.terrain);
        _currentTerrain.transform.position = new Vector3(0, 0, -3.0f);

        GameObject dogeAsset = Resources.Load<GameObject>("Prefabs/Doge");
        foreach (var dogInfo in config.dogs)
        {
            DogController dog = Instantiate(dogeAsset).GetComponent<DogController>();
            dog.transform.position = new Vector3(dogInfo.position.x, dogInfo.position.y, -2.0f);
            dog.SetDogeSimulated(false);
            _spawnedDogs.Add(dog);
        }

        GameObject combAsset = Resources.Load<GameObject>("Prefabs/Comb");
        foreach (var combInfo in config.combs)
        {
            CombController comb = Instantiate(combAsset).GetComponent<CombController>();
            comb.SetBeesToEmit(combInfo.bees);

            comb.transform.position = new Vector3(combInfo.positon.x, combInfo.positon.y, -1.0f);
            Vector3 pos = comb.transform.localScale;
            pos.x = combInfo.direction == LevelConfig.CombInfo.CombFacingDirection.Left ? -1 : 1;
            comb.transform.localScale = pos;

            _spawnedCombs.Add(comb);
        }

        lineDrawer.enabled = true;
        lineDrawer.AddOnDrawEndCallback((line) =>
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

            lineDrawer.enabled = false;
        });
    }

    public void ClearLevel()
    {
        for (int i = _spawnedDogs.Count - 1; i >= 0; i--) 
        {
            var doge = _spawnedDogs[i];
            _spawnedDogs.RemoveAt(i);
            Destroy(doge.gameObject);
        }

        for(int i = _spawnedCombs.Count - 1; i >= 0; i--) 
        {
            var comb = _spawnedCombs[i];
            _spawnedCombs.RemoveAt(i); 
            Destroy(comb.gameObject);
        }

        if(_currentTerrain != null)
        {
            Destroy(_currentTerrain);
        }
        if(_drawedLine != null)
        {
            Destroy(_drawedLine);
        }

        lineDrawer.enabled = false;
        lineDrawer.ClearOnDrawEndCallback();
    }
    #endregion

    #region Unity 消息
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        lineDrawer.enabled = false;

        _spawnedDogs = new List<DogController>();
        _spawnedCombs = new List<CombController>();
    }
    #endregion
}