using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GhostManager : Singleton<GhostManager>
{
    public GameObject GhostPrefab;

    public Transform[] GhostHome;

    public Transform[] target;

    public float StartDelayTime;

    public float EachTimeDelayTime;

    public int[] EachHomeGhostNum;

    public int maxGhostEachTarget;

    int currentSpawnWaveIdx;
    int currentSpawnTimeIdx;

    Dictionary<Transform, int> targetMap = new Dictionary<Transform, int>();

    private void Start()
    {
        foreach (var item in target)
        {
            item.GetComponent<GhostTarget>().OnGhostTargetDestoryEvent += GhostManager_OnGhostTargetDestoryEvent;
            targetMap.Add(item, 0);
        }

        StartGhostSpawnSequence();

    }

    private void GhostManager_OnGhostTargetDestoryEvent(GhostTarget target)
    {
        targetMap.Remove(target.transform);
    }

    public void StartGhostSpawnSequence()
    {
        Spawn();
    }

    public void Spawn()
    {
        SimpleTimerManager.instance.RunTimer(SpawnOne, EachTimeDelayTime);

        currentSpawnWaveIdx++;

        if (currentSpawnWaveIdx < EachHomeGhostNum.Length)
        {
            SimpleTimerManager.instance.RunTimer(Spawn, EachTimeDelayTime);
        }

    }

    public void SpawnOne()
    {
        foreach (var item in GhostHome)
        {
            GameObject enemy = Instantiate(GhostPrefab, item.position, Quaternion.identity);
            Ghost ghost = enemy.GetComponent<Ghost>();
            ghost.SetTarget(SelectTarget(ghost.gameObject));
            ghost.OnGhostKillEvent += Ghost_OnGhostKillEvent;
        }

        currentSpawnTimeIdx++;

        if (EachHomeGhostNum[currentSpawnWaveIdx] > currentSpawnTimeIdx)
        {
            SimpleTimerManager.instance.RunTimer(SpawnOne, EachTimeDelayTime);
        }
        else
        {
            Spawn();
        }

    }

    private void Ghost_OnGhostKillEvent(Ghost ghost)
    {
        targetMap[ghost.target] = targetMap[ghost.target] - 1;
    }

    public override bool ShouldDestoryOnLoad()
    {
        return true;
    }
    struct SortTarget
    {
        public Transform Target;
        public float Distance;
    }

    public Transform SelectTarget(GameObject self)
    {
        List<SortTarget> selectList = new List<SortTarget>();
        foreach (Transform item in targetMap.Keys)
        {
            if (targetMap[item] < maxGhostEachTarget)
            {
                SortTarget sort = new SortTarget();
                sort.Target = item;
                sort.Distance = Vector2.Distance(item.transform.position, self.transform.position);
                selectList.Add(sort);
            }
        }

        selectList.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        if(selectList.Count > 0)
        {
            return selectList[0].Target;
        }
        else
        {
            return target[Random.Range(0, target.Length)];
        }


    }

    public Transform SelectHome(GameObject self)
    {
        List<SortTarget> selectList = new List<SortTarget>();
        foreach (Transform item in GhostHome)
        {
            SortTarget sort = new SortTarget();
            sort.Target = item;
            sort.Distance = Vector2.Distance(item.transform.position, self.transform.position);
            selectList.Add(sort);
        }

        return selectList[0].Target;
    }
}
