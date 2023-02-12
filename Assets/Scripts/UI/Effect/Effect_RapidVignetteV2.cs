using UnityEngine;
using XPostProcessing;
using DG.Tweening;
using CarterGames.Assets.AudioManager;

public class Effect_RapidVignetteV2 : PPEffectBase
{
    [SerializeField] private float m_FadeTime = 0;
    [SerializeField] private float m_StartValue = 0;
    [SerializeField] private float m_EndValue = 0;
    [SerializeField] private string stage2Music;


    private RapidVignetteV2 m_VignetteV2 = null;
    private Tweener mCacheTweener = null;

    public override void Init()
    {
        base.Init();
        m_VignetteV2 = m_Control.profile.GetSetting<RapidVignetteV2>();
    }

    public override void StartModify()
    {
        base.StartModify();
        mCacheTweener = DoTweenExtension.DT_To(m_StartValue, m_EndValue, m_FadeTime, (float iValue) =>
        {
            m_VignetteV2.vignetteIndensity.value = iValue;
        });
    }
    public override void Finish()
    {
        mCacheTweener.KillTweener();
        AudioManager.instance.PlayBGMusic(5.0f, stage2Music);
        base.Finish();
    }

    public override void ResetData()
    {
        m_VignetteV2.vignetteIndensity.value = m_StartValue;
    }
}