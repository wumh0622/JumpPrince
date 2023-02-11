using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBody : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        if(rigidbody2D.velocity.magnitude > 0)
        {
            Vector2 dir = rigidbody2D.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
