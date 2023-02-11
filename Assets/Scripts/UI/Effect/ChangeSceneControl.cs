
using UnityEngine;

public class ChangeSceneControl : MonoBehaviour
{
    [SerializeField] private GameObject mChangeSceneEffectObj = null;
    [SerializeField] private UnityEngine.UI.Button mErrorBtn = null;

    private IModifyPPEffect[] mTotalChangeSceneEffect = null;
    private int mCurIndex = 0;

    private void Awake()
    {
        mTotalChangeSceneEffect = mChangeSceneEffectObj.GetComponentsInChildren<IModifyPPEffect>(true);
        for (int i = 0; i < mTotalChangeSceneEffect.Length; i++)
        {
            mTotalChangeSceneEffect[i].Init();
        }
        mErrorBtn.onClick.AddListener(mCurFinish);
    }
    [ContextMenu("Test")]
    public void StartChangeScene()
    {
        mCurIndex = 0;
        for (int i = 0; i < mTotalChangeSceneEffect.Length - 1; i++)
        {
            int aIndex = i;
            mTotalChangeSceneEffect[aIndex].SetNextAction(()=>{ 
                mTotalChangeSceneEffect[aIndex + 1].StartModify();
                mCurIndex++;
            });
        }
        mTotalChangeSceneEffect[0].StartModify();
    }

    public void mCurFinish()
    {
        mTotalChangeSceneEffect[mCurIndex].Finish();
    }
}