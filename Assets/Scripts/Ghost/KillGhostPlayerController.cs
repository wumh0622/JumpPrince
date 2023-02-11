using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGhostPlayerController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetButton("Fire1"))
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
    }

    public void DamageGhost(Ghost ghost)
    {
        ghost.DamageGhost();
    }
}
