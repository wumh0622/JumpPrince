using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTarget : MonoBehaviour
{
    public int NeedGhost;

    public int GhostCount;

    public float Speed;

    public Transform target;

    new Rigidbody2D rigidbody2D;

    float rightVelocity;
    float upVelocity;
    bool startMove;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!target || !startMove)
        {
            return;
        }

        if (Vector2.Distance(target.position, transform.position) > 0)
        {
            upVelocity = -Vector2.Dot(Vector2.up, (transform.position - target.position).normalized) * Speed * Time.deltaTime;
            rightVelocity = -Vector2.Dot(Vector2.right, (transform.position - target.position).normalized) * Speed * Time.deltaTime;
            GetComponent<Rigidbody2D>().velocity = new Vector2(rightVelocity, upVelocity);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void StartMove()
    {
        startMove = true;
    }

    public void EndMove()
    {
        startMove = false;
    }

    public void AddGhost(Ghost ghost)
    {
        ghost.OnGhostKillEvent += Ghost_OnGhostKillEvent;
        GhostCount++;
        if(GhostCount >= NeedGhost)
        {
            StartMove();
        }
    }

    public void Ghost_OnGhostKillEvent(Ghost ghost)
    {
        ghost.OnGhostKillEvent -= Ghost_OnGhostKillEvent;
        GhostCount--;
        if(GhostCount < NeedGhost)
        {
            EndMove();
        }
    }

}
