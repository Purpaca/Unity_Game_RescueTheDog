using Purpaca;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 播放音频测试 : MonoBehaviour
{
    string[] musList;
    // Start is called before the first frame update
    void Start()
    {
        musList = AssetManager.LoadMusicList();
        int id = Random.Range(0, musList.Length - 1);
        AudioClip clip = AssetManager.LoadMusic(musList[id]);
        AudioManager.Play(clip, -1, 1.0f, AudioManager.OutputChannel.Music);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}