using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPEffectBase: MonoBehaviour, IModifyPPEffect
{
    /// <summary>
    /// 如果<0 不自動銷毀
    /// </summary>
    [SerializeField] protected float m_KeepTime = 0;
    protected PostProcessVolume m_Control = null;
    protected System.Action m_NextAction = null;
    protected GameObject m_Obj = null;

    public virtual void Init()
    {
        m_Control = GetComponent<PostProcessVolume>();
        m_Obj = m_Control.gameObject;
        Close();
    }

    public virtual void StartModify()
    {
        ResetData();
        Open();
        if (m_KeepTime > 0)
        {
            SimpleTimerManager.instance.RunTimer(Finish, m_KeepTime);
        }
    }
    public virtual void Finish()
    {
        Close();
        m_NextAction?.Invoke();
    }

    public void SetNextAction(System.Action iAction)
    {
        m_NextAction = iAction;
    }

    public virtual void ResetData()
    { }


    private void Open()
    {
        m_Obj.SetActive(true);
    }
    private void Close()
    {
        m_Obj.SetActive(false);
    }
}