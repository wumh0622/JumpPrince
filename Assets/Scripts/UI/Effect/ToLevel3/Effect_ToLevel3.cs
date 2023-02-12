using UnityEngine;

public class Effect_ToLevel3 : PPEffectBase
{
    [SerializeField] private float m_SwitchTime = 0;
    [SerializeField] private GameObject m_BlueScreen = null;
    [SerializeField] private GameObject m_Camera2D = null;
    [SerializeField] private GameObject m_Camera2DLook = null;
    [SerializeField] private GameObject m_3DMap = null;

    public override void Init()
    {
        m_Obj = this.gameObject;
        this.gameObject.SetActive(false);
    }

    public override void StartModify()
    {
        base.StartModify();
        m_BlueScreen.SetActive(true);
        SimpleTimerManager.instance.RunTimer(() =>
        {
            SwitchLevel(true);

        }, m_SwitchTime);
    }

    public override void ResetData()
    {
        m_BlueScreen.SetActive(false);
        SwitchLevel(false);
    }

    private void SwitchLevel(bool iSwitch)
    {
        m_Camera2D.SetActive(!iSwitch);
        m_Camera2DLook.SetActive(iSwitch);
        m_3DMap.SetActive(iSwitch);
    }    
}