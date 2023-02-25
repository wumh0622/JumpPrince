using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using CarterGames.Assets.AudioManager;

public class GhostManager : Singleton<GhostManager>
{
    public GameObject GhostPrefab;

    public Transform[] GhostHome;

    public List<Transform> target = new List<Transform>();

    public float StartDelayTime;

    public float EachWaveDelayTime;
    public float EachSpawnDelayTime;

    public int[] EachHomeGhostNum;

    public int maxGhostEachTarget;

    int currentSpawnWaveIdx;
    int currentSpawnTimeIdx;

    Dictionary<Transform, int> targetMap = new Dictionary<Transform, int>();

    Transform mousePos;

    public float playTime;

    public UnityEvent OnStage2Over;
    public GameObject blueScreen;

    private void Start()
    {
        foreach (var item in target)
        {
            item.GetComponent<GhostTarget>().OnGhostTargetDestoryEvent += GhostManager_OnGhostTargetDestoryEvent;
            targetMap.Add(item, 0);
        }

        StartGhostSpawnSequence();
        mousePos = new GameObject().transform;

        SimpleTimerManager.instance.RunTimer(Stage2EndSequence, playTime);

    }

    public void Stage2EndSequence()
    {
        SimpleTimerManager.instance.RunTimer(Stage2End, 3.0f);
        blueScreen.SetActive(true);
        enabled = false;
    }

    public void Stage2End()
    {
        OnStage2Over.Invoke();
    }

    private void Update()
    {
        mousePos.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void GhostManager_OnGhostTargetDestoryEvent(GhostTarget ghostTarget)
    {
        targetMap.Remove(ghostTarget.transform);
        target.Remove(ghostTarget.transform);
    }

    public void StartGhostSpawnSequence()
    {
        SimpleTimerManager.instance.RunTimer(SpawnOne, EachSpawnDelayTime);
    }

    public void Spawn()
    {
        currentSpawnWaveIdx++;

        if (currentSpawnWaveIdx < EachHomeGhostNum.Length)
        {
            SimpleTimerManager.instance.RunTimer(SpawnOne, EachWaveDelayTime);
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
            SimpleTimerManager.instance.RunTimer(SpawnOne, EachSpawnDelayTime);
        }
        else
        {
            currentSpawnTimeIdx = 0;
            Spawn();
        }

    }

    private void Ghost_OnGhostKillEvent(Ghost ghost)
    {
        if(targetMap.ContainsKey(ghost.target))
        {
            targetMap[ghost.target] = targetMap[ghost.target] - 1;
        }
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

        if(targetMap.Count == 0)
        {
            return mousePos;
        }

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
            targetMap[selectList[0].Target] = targetMap[selectList[0].Target] + 1;
            return selectList[0].Target;
        }
        else
        {
            return target[Random.Range(0, target.Count)];
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
