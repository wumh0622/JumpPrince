using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SimpleTimerManager : Singleton<SimpleTimerManager>
{
    public Dictionary<System.Action, IEnumerator> timerList = new Dictionary<System.Action, IEnumerator>();

    float defaultFixedDeltaTime;
    bool slowDown;
    bool lockSlowDown;

    private void Awake()
    {
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        defaultFixedDeltaTime = Time.fixedUnscaledDeltaTime;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        System.Action[] copyKey = timerList.Keys.ToArray();
        foreach (System.Action action in copyKey)
        {
            StopTimer(action);
        }
        StopAllCoroutines();
        SlowDownEnd();
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
    }

    public void RunTimer(System.Action onStateOver ,float stateTimer, bool realTime = false)
    {
        if(timerList.ContainsKey(onStateOver))
        {
            StopCoroutine(timerList[onStateOver]);
            timerList.Remove(onStateOver);
        }

        IEnumerator newTimer;
        if (realTime)
        {
            newTimer = StateTimerUpdateRealTime(onStateOver, stateTimer);
        }
        else
        {
            newTimer = StateTimerUpdate(onStateOver, stateTimer);
        }
        timerList.Add(onStateOver, newTimer);
        StartCoroutine(newTimer);
    }

    public void StopTimer(System.Action onStateOver)
    {
        if (timerList.ContainsKey(onStateOver))
        {
            if(onStateOver.Target != null)
            {
                StopCoroutine(timerList[onStateOver]);
            }

            timerList.Remove(onStateOver);
        }
    }

    private IEnumerator StateTimerUpdate(System.Action onStateOver, float stateTimer)
    {
        yield return new WaitForSeconds(stateTimer);

        if(timerList.ContainsKey(onStateOver))
        {
            timerList.Remove(onStateOver);
        }
        else
        {
            yield return 0;
        }
        if(onStateOver.Target != null)
        {
            onStateOver();
        }
    }

    private IEnumerator StateTimerUpdateRealTime(System.Action onStateOver, float stateTimer)
    {
        yield return new WaitForSecondsRealtime(stateTimer);

        if (timerList.ContainsKey(onStateOver))
        {
            timerList.Remove(onStateOver);
        }
        onStateOver();
    }

    public void LockSlowDown(bool value)
    {
        lockSlowDown = value;
    }

    public void SlowDown(float slowDownSpeed, float slowDownTime)
    {
        if(lockSlowDown)
        {
            return;
        }
        Time.timeScale = slowDownSpeed;
        //Time.fixedDeltaTime = Time.timeScale * Time.fixedUnscaledDeltaTime;

        if(slowDownTime > 0)
        {
            RunTimer(SlowDownEnd, slowDownTime, true);
        }
    }

    private void SlowDownEnd()
    {
        lockSlowDown = false;
        Time.timeScale = 1.0f;
        //Time.fixedDeltaTime = defaultFixedDeltaTime;
    }
}
