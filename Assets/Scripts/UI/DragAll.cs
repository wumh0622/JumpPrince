using UnityEngine;

public class DragAll : MonoBehaviour
{
    [SerializeField] private LayerMask m_MovableLayers;

    private Transform m_DragObj = null;
    private Vector3 m_CacheOffset = Vector3.zero;
    private Camera m_CacheCamera = null;

    private void Awake()
    {
        m_CacheCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D aHit = Physics2D.Raycast(m_CacheCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                                                 float.PositiveInfinity, m_MovableLayers);
            if (aHit)
            {
                m_DragObj = aHit.transform;
                m_CacheOffset = m_DragObj.position - m_CacheCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_DragObj = null;
        }

        if (m_DragObj != null)
        {
            m_DragObj.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + m_CacheOffset;
        }
    }
}