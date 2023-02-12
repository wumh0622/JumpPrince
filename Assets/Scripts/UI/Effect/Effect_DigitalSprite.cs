using UnityEngine;
using CarterGames.Assets.AudioManager;

public class Effect_DigitalSprite : PPEffectBase
{
    public GameObject mErrorObj = null;

    public string openMusicID;
    public override void StartModify()
    {
        base.StartModify();

        AudioManager.instance.StopBGMusic(openMusicID);
        mErrorObj.SetActive(true);
    }

    public override void Finish()
    {
        base.Finish();
        mErrorObj.SetActive(false);
    }
}