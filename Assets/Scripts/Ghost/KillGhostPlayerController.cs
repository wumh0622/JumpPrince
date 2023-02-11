using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGhostPlayerController : MonoBehaviour
{
    public float Radius;
    public float Force;
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Ghost hitGhost = hit.collider.gameObject.GetComponent<Ghost>();
                if (hitGhost)
                {
                    DamageGhost(hitGhost);
                }
            }

        }

        if(Input.GetButtonDown("Fire2"))
        {
            Collider2D[] overlap = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Radius);
            foreach (var item in overlap)
            {
                Rigidbody2D rb = item.gameObject.GetComponent<Rigidbody2D>();
                if(rb)
                {
                    rb.AddForce((item.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized) * Force);
                }
            }
        }
    }

    public void DamageGhost(Ghost ghost)
    {
        ghost.DamageGhost();
    }
}
