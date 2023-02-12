
using UnityEngine;

public class ChangeSceneControl : MonoBehaviour
{
    [SerializeField] private GameObject mToLevel2EffectObj = null;
    [SerializeField] private GameObject mToLevel3EffectObj = null;
    [SerializeField] private UnityEngine.UI.Button mErrorBtn = null;

    private IModifyPPEffect[] mToLevel2Effect = null;
    private IModifyPPEffect[] mToLevel3Effect = null;
    private IModifyPPEffect mCurEffect = null;

    private void Awake()
    {
        mToLevel2Effect = mToLevel2EffectObj.GetComponentsInChildren<IModifyPPEffect>(true);
        mToLevel3Effect = mToLevel3EffectObj.GetComponentsInChildren<IModifyPPEffect>(true);
        for (int i = 0; i < mToLevel2Effect.Length; i++)
        {
            mToLevel2Effect[i].Init();
        }
        for (int i = 0; i < mToLevel3Effect.Length; i++)
        {
            mToLevel3Effect[i].Init();
        }
        mErrorBtn.onClick.AddListener(mCurFinish);
    }


    #region 測試用 之後刪除
    public int TestToLevelIndex = 2;
    [ContextMenu("Test")]
    void Test()
    {
        StartChangeScene(TestToLevelIndex);
    } 
    #endregion

    public void StartChangeScene(int iLevel)
    {
        switch (iLevel)
        {
            case (2):
                StartToLevel(mToLevel2Effect);
                break;
            case (3):
                StartToLevel(mToLevel3Effect);
                break;
            default:
                break;
        }
    }

    private void StartToLevel(IModifyPPEffect[] iModule)
    {
        mCurEffect = iModule[0];
        for (int i = 0; i < iModule.Length - 1; i++)
        {
            int aIndex = i;
            iModule[aIndex].SetNextAction(() => {
                iModule[aIndex + 1].StartModify();
                mCurEffect = iModule[aIndex + 1];
            });
        }
        iModule[0].StartModify();
    }        

    public void mCurFinish()
    {
        mCurEffect.Finish();
    }

    public void ResetData()
    {
        for (int i = 0; i < mToLevel2Effect.Length; i++)
        {
            mToLevel2Effect[i].ResetData();
        }
        for (int i = 0; i < mToLevel3Effect.Length; i++)
        {
            mToLevel3Effect[i].ResetData();
        }
    }
}