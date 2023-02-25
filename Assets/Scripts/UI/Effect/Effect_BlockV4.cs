using UnityEngine;
using XPostProcessing;
using DG.Tweening;
using CarterGames.Assets.AudioManager;

public partial class Effect_BlockV4 : PPEffectBase
{
    [SerializeField] private string audioEffect;
    [SerializeField] private string openMusicID;
    [SerializeField] private string openMusicID_Second;
    [SerializeField] private float m_FadeTime = 0;
    [SerializeField] private float m_StartValue = 0;
    [SerializeField] private float m_EndValue = 0;

    private GlitchImageBlockV4 m_BlockV4 = null;
    private Tweener mCacheTweener = null;

    new AudioSource audio;
    public override void Init()
    {
        base.Init();
        m_BlockV4 = m_Control.profile.GetSetting<GlitchImageBlockV4>();
    }

    public override void StartModify()
    {
        base.StartModify();
        AudioManager.instance.StopBGMusic(openMusicID);
        AudioManager.instance.StopBGMusic(openMusicID_Second);
        audio = AudioManager.instance.PlayAndGetSource(audioEffect);
        mCacheTweener = DoTweenExtension.DT_To(m_StartValue, m_EndValue, m_FadeTime, (float iValue) =>
        {
            m_BlockV4.MaxRGBSplitX.value = iValue;
            m_BlockV4.MaxRGBSplitY.value = iValue;
        });
    }
    public override void Finish()
    {
        if(audio)
        {
            audio.Stop();
        }

        mCacheTweener.KillTweener();
        base.Finish();
    }

    public override void ResetData()
    {
        m_BlockV4.MaxRGBSplitX.value = m_StartValue;
        m_BlockV4.MaxRGBSplitY.value = m_StartValue;
    }
}