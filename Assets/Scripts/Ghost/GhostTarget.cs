using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTarget : MonoBehaviour
{
    public int NeedGhost;

    public int GhostCount;

    public float Speed;

    public Transform target;

    public delegate void OnGhostTargetDestory(GhostTarget target);
    public event OnGhostTargetDestory OnGhostTargetDestoryEvent;

    new Rigidbody2D rigidbody2D;
    new Collider2D collider;
    float rightVelocity;
    float upVelocity;
    bool startMove;

    private void Awake()
    {
        gameObject.tag = "Dragable";
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!target || !startMove)
        {
            rigidbody2D.velocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance(target.position, transform.position) > 0.1f)
        {
            rigidbody2D.velocity = -(transform.position - target.position).normalized * Speed;
        }
        else
        {
            EndMove();
            TargetDestory();
            //GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void StartMove()
    {
        collider.isTrigger = true;
        startMove = true;
        target = GhostManager.instance.SelectHome(gameObject);
    }

    public void EndMove()
    {
        collider.isTrigger = false;
        startMove = false;
    }

    public void AddGhost(Ghost ghost)
    {
        ghost.OnGhostKillEvent += Ghost_OnGhostKillEvent;
        GhostCount++;
        gameObject.tag = "Untagged";
        if (GhostCount >= NeedGhost && !startMove)
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
        if(GhostCount == 0)
        {
            gameObject.tag = "Dragable";
        }
    }

    public void TargetDestory()
    {
        OnGhostTargetDestoryEvent?.Invoke(this);
        Destroy(gameObject);
    }

}
