using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ghost : MonoBehaviour
{
    public int Health;
    public float MoveSpeed;
    public float RotSpeed;
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
        //GetComponentInChildren<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
    }

    public void SetTarget(Transform setTarget)
    {
        isGetTarget = false;
        target = setTarget;
        GhostTarget ghostTarget = target.GetComponent<GhostTarget>();
        if (ghostTarget)
        {
            ghostTarget.OnGhostTargetDestoryEvent += GhostTarget_OnGhostTargetDestoryEvent;
        }
    }

    private void Update()
    {
        if (!target)
        {
            return;
        }

        //Vector2 dir = (transform.position - target.position).normalized;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector2 forward = transform.up * -1;
        Vector2 toOther = (target.transform.position - transform.position).normalized;

        float m_fVelocityForward = Vector2.Dot(forward, toOther);

        if (Mathf.Abs(m_fVelocityForward) > 0.1f)
        {

            rigidbody2D.AddTorque(m_fVelocityForward * -1 * RotSpeed * Time.deltaTime);

        }
        else
        {
            rigidbody2D.angularVelocity = 1.0f;
        }

        float fDistance = Vector3.Distance(target.transform.position, transform.position);
        if (Vector2.Distance(target.position, transform.position) > DamageRange && !isGetTarget)
        {
            rigidbody2D.AddForce(transform.right * MoveSpeed * Time.deltaTime);
        }
        else if (!isGetTarget)
        {
            GhostTarget ghostTarget = target.GetComponent<GhostTarget>();
            if (ghostTarget)
            {
                ghostTarget.AddGhost(this);

                isGetTarget = true;
                rigidbody2D.velocity = Vector2.zero;
                posOffset = transform.position - target.position;
            }
        }



        if (isGetTarget)
        {
            transform.position = (Vector2)target.position + posOffset;
        }
    }

    private void GhostTarget_OnGhostTargetDestoryEvent(GhostTarget target)
    {
        SetTarget(GhostManager.instance.SelectTarget(gameObject));
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

        GhostTarget ghostTarget = target.GetComponent<GhostTarget>();
        if (ghostTarget)
        {
            ghostTarget.OnGhostTargetDestoryEvent -= GhostTarget_OnGhostTargetDestoryEvent;
        }

        Destroy(gameObject);
    }
}
