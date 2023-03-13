using UnityEngine;

public class EffectContainer : MonoBehaviour
{
    public byte mLevelIndex = 0;
    private IModifyPPEffect[] mToLevelEffect = null;
    private IModifyPPEffect mCurEffect = null;

    public void Init()
    {
        mToLevelEffect = this.gameObject.GetComponentsInChildren<IModifyPPEffect>(true);
        for (int i = 0; i < mToLevelEffect.Length; i++)
        {
            mToLevelEffect[i].Init();
        }
    }

    public void ToLevel()
    {
        mCurEffect = mToLevelEffect[0];
        for (int i = 0; i < mToLevelEffect.Length - 1; i++)
        {
            int aIndex = i;
            mToLevelEffect[aIndex].SetNextAction(() => {
                mToLevelEffect[aIndex + 1].StartModify();
                mCurEffect = mToLevelEffect[aIndex + 1];
            });
        }
        mToLevelEffect[0].StartModify();
    }

    public void Finish()
    {
        mCurEffect.Finish();
        mCurEffect = null;
    }

    public void ResetData()
    {
        for (int i = 0; i < mToLevelEffect.Length; i++)
        {
            mToLevelEffect[i].ResetData();
        }
    }
}