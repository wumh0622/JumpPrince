using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ghost : MonoBehaviour
{
    public int Health;
    public float Speed;
    public float Damage;
    public float DamageRange;

    public Transform target;

    public delegate void OnGhostKill(Ghost ghost);
    public event OnGhostKill OnGhostKillEvent;

    new Rigidbody2D rigidbody2D;

    float upVelocity;
    float rightVelocity;
    bool isGetTarget;
    Vector2 posOffset;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Transform setTarget)
    {
        target = setTarget;
    }

    private void Update()
    {
        if(!target)
        {
            return;
        }

        Vector2 dir = (transform.position - target.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (!isGetTarget && Vector2.Distance(target.position, transform.position) > DamageRange)
        {
            rigidbody2D.velocity = -transform.right * Speed;
        }
        else if(!isGetTarget)
        {
            isGetTarget = true;
            rigidbody2D.velocity = Vector2.zero;
            posOffset = transform.position - target.position;
        }

        if(isGetTarget)
        {
            transform.position = (Vector2)target.position + posOffset;
        }
    }

    public void DamageGhost()
    {
        Health--;
        if(Health == 0)
        {
            KillGhost();
        }
    }

    public void KillGhost()
    {
        OnGhostKillEvent?.Invoke(this);
        Destroy(gameObject);
    }
}
