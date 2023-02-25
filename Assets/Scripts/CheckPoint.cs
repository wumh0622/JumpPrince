using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int Index;
    public Vector2 safePointOffset;
    public float monsterHeightOffset;

    public Stage1Monster monster;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)safePointOffset, 0.3f);
        if(monster)
        {
            Gizmos.DrawWireSphere(new Vector3(monster.transform.position.x, transform.position.y + monsterHeightOffset, transform.position.z), 0.3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(SafePointManager.instance.currentCheckPoint)
        {
            if (SafePointManager.instance.currentCheckPoint.Index < Index)
            {
                SafePointManager.instance.currentCheckPoint = this;
            }
        }
        else
        {
            SafePointManager.instance.currentCheckPoint = this;
        }
    }

    public Vector2 GetSafePoint()
    {
        return (Vector2)transform.position + safePointOffset;
    }

    public Vector2 GetMonsterPoint()
    {
        return new Vector2(monster.transform.position.x, transform.position.y + monsterHeightOffset);
    }
}
