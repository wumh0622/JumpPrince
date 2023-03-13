using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChangeSceneControl : MonoBehaviour
{
    [SerializeField] private GameObject mEffectObjPos = null;
    [SerializeField] private UnityEngine.UI.Button mErrorBtn = null;

    private Dictionary<int, EffectContainer> mEffectDic = null;
    private EffectContainer mCurEffect = null;

    private void Awake()
    {
        mEffectDic = new Dictionary<int, EffectContainer>();
        EffectContainer[] aCacheEffects = mEffectObjPos.GetComponentsInChildren<EffectContainer>(true);
        for (int i = 0; i < aCacheEffects.Length; i++)
        {
            aCacheEffects[i].Init();
            mEffectDic.Add(aCacheEffects[i].mLevelIndex, aCacheEffects[i]);
        }
        mErrorBtn.onClick.AddListener(CurFinish);
    }

    #region 測試用 之後可刪除
    public int TestToLevelIndex = 2;
    [ContextMenu("Test")]
    void Test()
    {
        StartChangeScene(TestToLevelIndex);
    } 
    #endregion

    public void StartChangeScene(int iLevel)
    {
        Assert.IsTrue(mEffectDic.Count != 0 || mEffectDic.ContainsKey(iLevel), $"mEffectDic == null  or mEffectDic NotKey  Key={iLevel}");
        mCurEffect = mEffectDic[iLevel];
        ResetData();
        mCurEffect.ToLevel();
    }

    public void CurFinish()
    {
        Assert.IsTrue(mCurEffect != null, "mCurEffect==null");
        mCurEffect.Finish();
        mCurEffect = null;
    }

    private void ResetData()
    {
        Assert.IsTrue(mCurEffect != null, "mCurEffect==null");
        mCurEffect.ResetData();
    }
}