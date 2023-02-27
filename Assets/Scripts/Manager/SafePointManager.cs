using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePointManager : Singleton<SafePointManager>
{
    public CheckPoint currentCheckPoint;
    public Stage1Monster monster;

    public float respawnTime = 1.5f;

    PlayerController controller;
    

    private void Awake()
    {
        controller = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {
    }

    public void OnPlayerDied() 
    {
        StartCoroutine(WaitForReset());
    }

    IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(respawnTime);

        Respawn();
        ResetMonster();
    }

    private void Respawn()
    {
        controller.Teleport(currentCheckPoint.GetSafePoint());
        controller.Respawn();
    }

    private void ResetMonster()
    {
        monster.MonsterTeleport(currentCheckPoint.GetMonsterPoint());
    }
}
