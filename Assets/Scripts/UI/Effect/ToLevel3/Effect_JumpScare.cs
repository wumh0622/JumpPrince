using UnityEngine;
using CarterGames.Assets.AudioManager;
using XPostProcessing;
using DG.Tweening;

public class Effect_JumpScare : PPEffectBase
{
    [SerializeField] private string m_ScareAudio = string.Empty;
    [SerializeField] private GameObject m_MonsterObj = null;
    [SerializeField] private GameObject m_MaskObj = null;

    [Header("時間設定")]
    [Header("出現怪物前延遲")]
    [SerializeField] private float m_DelayTime = 0;
    [Header("怪物出現後模糊前延遲")]
    [SerializeField] private float m_BlurDelayTime = 0;
    [Header("模糊延遲")]
    [SerializeField] private float m_ModifyDelay = 0;

    [Header("模糊設定值")]    
    [SerializeField] private float m_BlurRadiusStart = 0;
    [SerializeField] private float m_BlurRadiusEnd = 0;
    [SerializeField] private int m_IterationStart = 0;
    [SerializeField] private int m_IterationEnd = 0;

    private BokehBlur m_BokehBlur = null;
    private Tweener mCacheTweener_1 = null;
    private Tweener mCacheTweener_2 = null;

    public override void Init()
    {
        base.Init();
        m_BokehBlur = m_Control.profile.GetSetting<BokehBlur>();
    }

    public override void StartModify()
    {
        SimpleTimerManager.instance.RunTimer(() =>
        {
            OpenMonster();
        }, m_DelayTime);
    }

    public override void Finish()
    {
        mCacheTweener_1.KillTweener();
        mCacheTweener_2.KillTweener();
        m_MaskObj.SetActive(true);
        base.Finish();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public override void ResetData()
    {
        m_BokehBlur.BlurRadius.value = m_BlurRadiusStart;
        m_BokehBlur.Iteration.value = m_IterationStart;
    }

    private void OpenMonster()
    {
        base.StartModify();
        AudioManager.instance.Play(m_ScareAudio);
        m_MonsterObj.SetActive(true);

        SimpleTimerManager.instance.RunTimer(() =>
        {
            ModifyEffect();
        }, m_BlurDelayTime);
    }

    private void ModifyEffect()
    {
        mCacheTweener_1 = DoTweenExtension.DT_To(m_BlurRadiusStart, m_BlurRadiusEnd, m_ModifyDelay, (float iValue) =>
        {
            m_BokehBlur.BlurRadius.value = iValue;
        });
        mCacheTweener_2 = DoTweenExtension.DT_To(m_IterationStart, m_IterationEnd, m_ModifyDelay, (int iValue) =>
        {
            m_BokehBlur.Iteration.value = iValue;
        });
    }
}