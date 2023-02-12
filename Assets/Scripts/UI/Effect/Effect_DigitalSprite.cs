using UnityEngine;
using CarterGames.Assets.AudioManager;

public class Effect_DigitalSprite : PPEffectBase
{
    public GameObject mErrorObj = null;
    public override void StartModify()
    {
        base.StartModify();

        mErrorObj.SetActive(true);
    }

    public override void Finish()
    {
        base.Finish();
        mErrorObj.SetActive(false);
    }
}