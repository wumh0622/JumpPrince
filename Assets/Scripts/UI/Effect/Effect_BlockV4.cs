using UnityEngine;
using XPostProcessing;
using DG.Tweening;

public partial class Effect_BlockV4 : PPEffectBase
{
    [SerializeField] private float m_FadeTime = 0;
    [SerializeField] private float m_StartValue = 0;
    [SerializeField] private float m_EndValue = 0;

    private GlitchImageBlockV4 m_BlockV4 = null;
    private Tweener mCacheTweener = null;

    public override void Init()
    {
        base.Init();
        m_BlockV4 = m_Control.profile.GetSetting<GlitchImageBlockV4>();
    }

    public override void StartModify()
    {
        base.StartModify();
        mCacheTweener = DoTweenExtension.DT_To(m_StartValue, m_EndValue, m_FadeTime, (float iValue) =>
        {
            m_BlockV4.MaxRGBSplitX.value = iValue;
            m_BlockV4.MaxRGBSplitY.value = iValue;
        });
    }
    public override void Finish()
    {
        mCacheTweener.KillTweener();
        base.Finish();
    }

    public override void ResetData()
    {
        m_BlockV4.MaxRGBSplitX.value = m_StartValue;
        m_BlockV4.MaxRGBSplitY.value = m_StartValue;
    }
}