using UnityEngine;

public class Effect_OpenMaskDelay : PPEffectBase
{
    [SerializeField] private GameObject m_MaskObj = null;

    public override void Init()
    {
        m_Obj = this.gameObject;
        this.gameObject.SetActive(false);
    }

    public override void StartModify()
    {
        m_MaskObj.SetActive(true);
        base.StartModify();
    }

    public override void Finish()
    {
        base.Finish();
        m_MaskObj.SetActive(false);
    }
}
