using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePointManager : Singleton<SafePointManager>
{
    public CheckPoint currentCheckPoint;
    public Stage1Monster monster;

    PlayerController controller;
    

    private void Awake()
    {
        controller = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        controller.Teleport(currentCheckPoint.GetSafePoint());
    }

    public void ResetMonster()
    {
        monster.MonsterTeleport(currentCheckPoint.GetMonsterPoint());
    }
}
