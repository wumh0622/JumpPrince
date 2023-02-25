using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarterGames.Assets.AudioManager;

public class StartBGMusic : MonoBehaviour
{
    public string musicName;

    public float startFade;
    // Start is called before the first frame update
    void Start()
    {
        if(musicName != "")
        {
            AudioManager.instance.PlayBGMusic(startFade, musicName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
