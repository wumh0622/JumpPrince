using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicObject : MonoBehaviour
{
    /// <summary>
    /// The minimum normal (dot product) considered suitable for the entity sit on.
    /// </summary>
    public float minGroundNormalY = .65f;

    /// <summary>
    /// A custom gravity coefficient applied to this entity.
    /// </summary>
    public float gravityModifier = 1f;

    /// <summary>
    /// The current velocity of the entity.
    /// </summary>
    public Vector2 velocity;

    public float groundFloorDist = 0.3f;

    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;

    protected Collider2D selfCollider;

    protected ContactFilter2D contactFilter;
    protected Collider2D[] overlapBuffer = new Collider2D[16];

    protected Transform groundTransform;
    protected Vector2 groundRelativePosition;

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    public bool IsGrounded()
    {
        return groundTransform is not null;
    }
    public void Teleport(Vector2 position)
    {
        groundTransform = null;
        gameObject.transform.Translate(new Vector3(position.x, position.y) - gameObject.transform.position);
    }

    public void Launch(Vector2 force)
    {
        groundTransform = null;
        velocity = force;
    }

    protected virtual void OnEnable()
    {
        selfCollider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        selfCollider.enabled = true;

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(1);
        contactFilter.useLayerMask = true;

        groundNormal = Vector2.up;
    }

    protected virtual void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (IsGrounded())
        {
            Vector2 groundDelta = groundTransform.TransformPoint(groundRelativePosition) - gameObject.transform.position;
            gameObject.transform.Translate(new Vector3(groundDelta.x, groundDelta.y));
        }

        PushOutPenetration();

        if (IsGrounded())
        {
            velocity = targetVelocity;

            Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
            Vector2 selfDelta = velocity.x * Time.deltaTime * moveAlongGround;
            PerformMovement(selfDelta);
            CheckFloor(groundFloorDist);
        }
        else
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

            Vector2 delta = velocity * Time.deltaTime;
            PerformMovement(delta);

            if (Vector2.Dot(velocity, Physics2D.gravity) > 0)
            {
                CheckFloor(shellRadius);
            }
        }
    }

    void PushOutPenetration()
    {
        int count = selfCollider.OverlapCollider(contactFilter, overlapBuffer);
        for (int i = 0; i < count; ++i)
        {
            if (overlapBuffer[i].isTrigger)
            {
                continue;
            }

            ColliderDistance2D Penetration = selfCollider.Distance(overlapBuffer[i]);
            if (Penetration.isOverlapped)
            {
                Vector2 push = Penetration.normal * Penetration.distance;
                gameObject.transform.Translate(new Vector3(push.x, push.y));
            }
        }
    }

    void PerformMovement(Vector2 move)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(selfCollider.bounds.center, selfCollider.bounds.extents * 2, transform.eulerAngles.z, move, distance + shellRadius, 1); ;
            int hitIndex = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.isTrigger)
                {
                    continue;
                }

                float modifiedDistance = hits[i].distance - shellRadius;
                if (modifiedDistance < distance)
                {
                    distance = modifiedDistance;
                    hitIndex = i;
                }
            }

            if (IsGrounded() == false &&
                hitIndex >= 0)
            {
                Vector2 normal = hits[hitIndex].normal;
                float gravityDot = Vector2.Dot(Physics2D.gravity.normalized, normal);
                if (gravityDot > -minGroundNormalY)
                {
                    float velocityDot = Vector2.Dot(velocity, normal);
                    if (velocityDot < 0)
                    {
                        velocity = Vector2.Reflect(velocity, normal);
                    }
                }
            }
        }

        Vector2 delta = move.normalized * distance;
        gameObject.transform.Translate(new Vector3(delta.x, delta.y));
    }

    void CheckFloor(float floorDist)
    {
        float distance = selfCollider.bounds.extents.y + floorDist;

        groundTransform = null;

        Vector2 size = new Vector2(selfCollider.bounds.extents.x * 2, shellRadius);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(selfCollider.bounds.center, size, transform.eulerAngles.z, Physics2D.gravity, distance, 1);
        int hitIndex = -1;
        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i].collider.isTrigger)
            {
                continue;
            }

            if (hits[i].distance < distance)
            {
                distance = hits[i].distance;

                Vector2 normal = hits[i].normal;
                float gravityDot = Vector2.Dot(Physics2D.gravity.normalized, normal);
                if (gravityDot <= -minGroundNormalY)
                {
                    hitIndex = i;
                }
            }
        }

        if (hitIndex >= 0)
        {
            gameObject.transform.Translate(Physics2D.gravity.normalized * (distance - selfCollider.bounds.extents.y - Physics2D.defaultContactOffset * 2));

            groundTransform = hits[hitIndex].collider.transform;
            groundNormal = hits[hitIndex].normal;
            groundRelativePosition = groundTransform.InverseTransformPoint(gameObject.transform.position);
        }
    }
}