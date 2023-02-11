using System;
using DG.Tweening;

public static class DoTweenExtension 
{
    public static Tweener DT_To(float iStart, float iEnd, float iTime, Action<float> iDoEvent)
    {
        float aValue = iStart;
        return DOTween.To(() => aValue, x => aValue = x, iEnd, iTime).OnUpdate(() =>
        {
            iDoEvent(aValue);
        });
    }

    public static void KillTweener(this Tweener iTweener)
    {
        if (iTweener != null)
        {
            iTweener.Kill(true);
            iTweener = null;
        }
    }
}