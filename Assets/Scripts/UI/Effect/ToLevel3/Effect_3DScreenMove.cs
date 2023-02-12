using UnityEngine;

public class Effect_3DScreenMove : PPEffectBase
{
    [SerializeField] private Animator m_MoveAnim = null;

    public override void Init()
    {
        m_Obj = this.gameObject;
        this.gameObject.SetActive(false);
    }

    public override void StartModify()
    {
        base.StartModify();
        m_MoveAnim.Play("3DCameraMove");
    }
}